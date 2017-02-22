using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.Services;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using DotNet.Business;
using System.Windows.Forms;
using WafTraffic.Applications.Common;
using DotNet.Utilities;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class LbConfigController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private LbConfigUpdateViewModel mConfigUpdateViewModel;
        private LbConfigQueryViewModel mConfigQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;

        private List<ZdtzConfigTable> mParentNodeList;

        [ImportingConstructor]
        public LbConfigController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mConfigUpdateViewModel = container.GetExportedValue<LbConfigUpdateViewModel>();
                mConfigQueryViewModel = container.GetExportedValue<LbConfigQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Initialize()
        {
            try
            {
                mConfigUpdateViewModel.SaveCommand = this.mSaveCommand;
                mConfigUpdateViewModel.CancelCommand = this.mCancelCommand;

                mConfigQueryViewModel.NewCommand = this.mNewCommand;
                mConfigQueryViewModel.ModifyCommand = this.mModifyCommand;
                mConfigQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mConfigQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mConfigQueryViewModel.QueryCommand = this.mQueryCommand;

                InitParentNodeList();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void InitParentNodeList()
        {
            mParentNodeList = mEntityService.QueryConfigParentNodes();
            if(mParentNodeList != null)
            {
                // 添加根节点
                ZdtzConfigTable root = new ZdtzConfigTable();
                root.Id = 0;
                root.Title = "根节点";
                root.ParentId = 0;
                root.NodeLevel = 0;
                root.Code = "0";
                root.IsDeleted = 0;
                mParentNodeList.Insert(0, root);
            }
            mConfigUpdateViewModel.ParentNodeList = mParentNodeList;
        }

        public bool NewOper()
        {
            bool newer = true;

            try
            {
                if (!HaveAuth()) return false;

                mConfigUpdateViewModel.IsBrowse = false;
                mConfigUpdateViewModel.IsNodeLevelEnabled = true;
                mConfigUpdateViewModel.ConfigEntity = new ZdtzConfigTable();

                mMainFrameViewModel.ContentView = mConfigUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;

            try
            {
                if (!HaveAuth()) return false;

                mConfigUpdateViewModel.IsBrowse = false;
                mConfigUpdateViewModel.IsNodeLevelEnabled = false;
                mConfigUpdateViewModel.ConfigEntity = mConfigQueryViewModel.SelectedConfig;
                if (mConfigUpdateViewModel.ParentNodeList != null)
                {
                    mConfigUpdateViewModel.SelectedParent = mConfigUpdateViewModel.ParentNodeList.Find(p => p.Id == mConfigQueryViewModel.SelectedConfig.ParentId);
                }

                mMainFrameViewModel.ContentView = mConfigUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool BrowseOper()
        {
            bool newer = true;

            try
            {
                if (!HaveAuth()) return false;

                mConfigUpdateViewModel.IsBrowse = true;
                mConfigUpdateViewModel.IsNodeLevelEnabled = false;
                mConfigUpdateViewModel.ConfigEntity = mConfigQueryViewModel.SelectedConfig;
                if (mConfigUpdateViewModel.ParentNodeList != null)
                {
                    mConfigUpdateViewModel.SelectedParent = mConfigUpdateViewModel.ParentNodeList.Find(p => p.Id == mConfigQueryViewModel.SelectedConfig.ParentId);
                }
                
                mMainFrameViewModel.ContentView = mConfigUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DeleteOper()
        {            
            bool deleter = false;

            try
            {
                if (!HaveAuth()) return false;

                if (mEntityService.QueryConfigUsingCount(mConfigQueryViewModel.SelectedConfig.Id) > 0)
                {
                    MessageBox.Show("有其他台账使用该台账为上级节点，不可删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (mEntityService.QueryStaticUsingCount(mConfigQueryViewModel.SelectedConfig.Id) > 0)
                {
                    MessageBox.Show("该台账类型仍含台账数据，不可删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    mConfigQueryViewModel.SelectedConfig.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mConfigQueryViewModel.SelectedConfig.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mConfigQueryViewModel.SelectedConfig.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    InitParentNodeList();
                    mConfigQueryViewModel.GridRefresh();
                }
                deleter = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return deleter;
        }

        public bool CancelOper()
        {
            bool newer = true;

            try
            {
                /*
                if (mConfigQueryViewModel.SelectedConfig != null && mConfigQueryViewModel.SelectedConfig.Id != 0)
                {
                    mEntityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, mConfigQueryViewModel.SelectedConfig);
                }
                */
                mConfigUpdateViewModel.SelectedParent = null;
                mMainFrameViewModel.ContentView = mConfigQueryViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool QueryOper()
        {
            bool newer = true;
            string inputTitle;
            try
            {
                if (!HaveAuth()) return false;

                inputTitle = mConfigQueryViewModel.InputTitle;

                mConfigQueryViewModel.LbConfigs = mEntityService.QueryLbConfigs(inputTitle);
                if (mConfigQueryViewModel.LbConfigs == null || mConfigQueryViewModel.LbConfigs.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mConfigQueryViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool Save()
        {
            bool saved = false;

            try
            {
                if (!HaveAuth()) return false;

                if (!ValueCheck())
                {
                    return saved;
                }
                if (mConfigUpdateViewModel.ConfigEntity.Id > 0)
                {
                    mConfigUpdateViewModel.ConfigEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mConfigUpdateViewModel.ConfigEntity.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    mConfigUpdateViewModel.ConfigEntity.Id = mEntityService.QueryConfigMaxId() + 1;
                    mConfigUpdateViewModel.ConfigEntity.ParentId = mConfigUpdateViewModel.SelectedParent.Id;
                    mConfigUpdateViewModel.ConfigEntity.NodeLevel = mConfigUpdateViewModel.SelectedParent.NodeLevel + 1;
                    //mConfigUpdateViewModel.ConfigEntity.LogbookType = mConfigUpdateViewModel.SelectedParent.LogbookType;
                    mConfigUpdateViewModel.ConfigEntity.Code = GetCodeByParent(mConfigUpdateViewModel.SelectedParent.Id, mConfigUpdateViewModel.SelectedParent.Code);

                    mConfigUpdateViewModel.ConfigEntity.CreateTime = System.DateTime.Now;
                    mConfigUpdateViewModel.ConfigEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mConfigUpdateViewModel.ConfigEntity.UpdateTime = System.DateTime.Now;
                    mConfigUpdateViewModel.ConfigEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.Entities.ZdtzConfigTables.AddObject(mConfigUpdateViewModel.ConfigEntity);
                }
                mEntityService.Entities.SaveChanges();
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                InitParentNodeList();
                mMainFrameViewModel.ContentView = mConfigQueryViewModel.View;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return saved;
        }

        private string GetCodeByParent(int parentId, string parentCode)
        {
            string code = string.Empty;
            string prefixCode = string.Empty;

            code = mEntityService.QueryConfigMaxCodeByParent(parentId);
            if (!string.IsNullOrEmpty(code))
            {
                if (code.Contains('.'))
                {
                    prefixCode = code.Substring(0, code.IndexOf('.')+1);
                    code = code.Substring(code.IndexOf('.')+1);
                }

                int iCode = Convert.ToInt32(code);
                if (iCode != 0)
                {
                    iCode++;
                    code = prefixCode + iCode.ToString();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(parentCode))
                {
                    code = parentCode + "." + YcConstants.STR_LBCONFIG_CODE_START;
                }
            }

            return code;
        }

        private bool ValueCheck()
        {
            bool result = true;

            if (mConfigUpdateViewModel.IsNodeLevelEnabled && mConfigUpdateViewModel.SelectedParent == null)
            {
                MessageBox.Show("上级节点是必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(mConfigUpdateViewModel.ConfigEntity.Title))
            {
                MessageBox.Show("台账名称是必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return result;
        }

        private bool HaveAuth()
        {
            bool haveAuth = CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator;
            if (!haveAuth)
            {
                MessageBox.Show("无操作权限", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return haveAuth;
        }
    }
}
    