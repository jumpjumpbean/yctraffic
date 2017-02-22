namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class UserReportManager : BaseManager, IWorkFlowManager
    {
        public UserReportManager()
        {
        }

        public UserReportManager(BaseUserInfo userInfo)
        {
            base.UserInfo = userInfo;
        }

        public int AfterAutoStatr(string id)
        {
            return 0;
        }

        public int AuditQuash(string[] ids, string auditIdea)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                num += this.AuditQuash(ids[i], auditIdea);
            }
            return num;
        }

        public int AuditQuash(string id, string auditIdea)
        {
            return 0;
        }

        public int AutoStatr(string[] ids, string auditIdea)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (!string.IsNullOrEmpty(this.AutoStatr(ids[i], auditIdea, (string) null)))
                {
                    num++;
                }
            }
            return num;
        }

        public string AutoStatr(string id, string auditIdea, DataTable dtWorkFlowActivity)
        {
            return string.Empty;
        }

        //public string AutoStatr(string id, string auditIdea, string toUserId = null)  //C# 4.0 才支持缺省参数
        public string AutoStatr(string id, string auditIdea, string toUserId)
        {
            string str = string.Empty;
            WorkFlowInfo info = this.BeforeAutoStatr(id);
            if (info != null)
            {
                string categoryCode = info.CategoryCode;
                string categoryFullName = info.CategoryFullName;
                string objectFullName = info.ObjectFullName;
                string workFlowCode = info.WorkFlowCode;
                str = new BaseWorkFlowCurrentManager(base.DbHelper, base.UserInfo).AutoStatr(this, categoryCode, categoryFullName, id, objectFullName, workFlowCode, auditIdea, toUserId);
                if (!string.IsNullOrEmpty(str))
                {
                    this.AfterAutoStatr(id);
                }
            }
            return str;
        }

         //用来模拟默认参数
        public string AutoStatr(string id, string auditIdea)
        {
            string toUserId = null;
            return AutoStatr(id,auditIdea,toUserId);
        }

        public WorkFlowInfo BeforeAutoStatr(string id)
        {
            return null;
        }

        public IDbHelper GetDbHelper()
        {
            return base.DbHelper;
        }

        public string GetUrl(string currentId)
        {
            string webHostUrl = BaseSystemInfo.WebHostUrl;
            if (string.IsNullOrEmpty(webHostUrl))
            {
                webHostUrl = "WebHostUrl";
            }
            return ("{" + webHostUrl + "}Work.aspx?OpenId={OpenId}&ModuleCode=WorkFlow&Left=LeftMenu.aspx");
        }

        public BaseUserInfo GetUserInfo()
        {
            return base.UserInfo;
        }

        public bool OnAuditComplete(string currentId, string categoryCode, string auditIdea)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string objectId = manager.GetEntity(currentId).ObjectId;
            if (!string.IsNullOrEmpty(objectId))
            {
                IDbHelper dbHelper = new SqlHelper(BaseSystemInfo.BusinessDbConnection);
                dbHelper.Open();
                SQLBuilder builder = new SQLBuilder(dbHelper);
                switch (categoryCode)
                {
                    case "PuTongCaiGouDan":
                    case "GuoNeiCaiGouHeTong":
                    case "PutongCaiGouDanDGM":
                    case "PutongCaiGouDanManager":
                        builder.BeginUpdate("WL物品申购");
                        builder.SetValue("AuditStatus", AuditStatus.AuditComplete.ToString(), null);
                        builder.SetValue("审核", 1, null);
                        builder.SetDBNow("审核日期");
                        builder.SetValue("审核员", BaseSystemInfo.UserInfo.Code, null);
                        builder.SetWhere("申请单号", objectId);
                        builder.EndUpdate();
                        break;

                    case "YuanFuCaiLiaoShenQingDan":
                        builder.BeginUpdate("WL部门物品申购");
                        builder.SetValue("AuditStatus", AuditStatus.AuditComplete.ToString(), null);
                        builder.SetValue("审核", 1, null);
                        builder.SetValue("总审核", 1, null);
                        builder.SetDBNow("审核日期");
                        builder.SetDBNow("总审核日期");
                        builder.SetValue("审核员", BaseSystemInfo.UserInfo.Code, null);
                        builder.SetWhere("申购单号", objectId);
                        builder.EndUpdate();
                        break;

                    case "MoJuCaiGouHeTongP":
                    case "MoJuCaiGouHeTongGM":
                        builder.BeginUpdate("GCMJ模具申请");
                        builder.SetValue("AuditStatus", AuditStatus.AuditComplete.ToString(), null);
                        builder.SetValue("审核", 1, null);
                        builder.SetWhere("申请单号", objectId);
                        builder.EndUpdate();
                        break;
                }
                dbHelper.Close();
            }
            return true;
        }

        public bool OnAuditQuash(string[] currentIds, string categoryCode, string auditIdea)
        {
            for (int i = 0; i < currentIds.Length; i++)
            {
                this.OnAuditQuash(currentIds[i], categoryCode, auditIdea);
            }
            return true;
        }

        public bool OnAuditQuash(string currentId, string categoryCode, string auditIdea)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string objectId = manager.GetEntity(currentId).ObjectId;
            if (!string.IsNullOrEmpty(objectId))
            {
                IDbHelper dbHelper = new SqlHelper(BaseSystemInfo.BusinessDbConnection);
                dbHelper.Open();
                SQLBuilder builder = new SQLBuilder(dbHelper);
                switch (categoryCode)
                {
                    case "PuTongCaiGouDan":
                    case "GuoNeiCaiGouHeTong":
                    case "PutongCaiGouDanDGM":
                    case "PutongCaiGouDanManager":
                        builder.BeginUpdate("WL物品申购");
                        builder.SetDBNow("审核日期");
                        builder.SetValue("审核员", BaseSystemInfo.UserInfo.Code, null);
                        builder.SetValue("AuditStatus", AuditStatus.AuditQuash.ToString(), null);
                        builder.SetWhere("申请单号", objectId);
                        builder.EndUpdate();
                        break;

                    case "YuanFuCaiLiaoShenQingDan":
                        builder.BeginUpdate("WL部门物品申购");
                        builder.SetValue("AuditStatus", AuditStatus.AuditQuash.ToString(), null);
                        builder.SetWhere("申购单号", objectId);
                        builder.EndUpdate();
                        break;

                    case "MoJuCaiGouHeTongP":
                    case "MoJuCaiGouHeTongGM":
                        builder.BeginUpdate("GCMJ模具申请");
                        builder.SetValue("AuditStatus", AuditStatus.AuditQuash.ToString(), null);
                        builder.SetWhere("申请单号", objectId);
                        builder.EndUpdate();
                        break;
                }
                dbHelper.Close();
            }
            return true;
        }

        public bool OnAuditReject(string currentId, string categoryCode, string auditIdea)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string objectId = manager.GetEntity(currentId).ObjectId;
            if (!string.IsNullOrEmpty(objectId))
            {
                IDbHelper dbHelper = new SqlHelper(BaseSystemInfo.BusinessDbConnection);
                dbHelper.Open();
                SQLBuilder builder = new SQLBuilder(dbHelper);
                switch (categoryCode)
                {
                    case "PuTongCaiGouDan":
                    case "GuoNeiCaiGouHeTong":
                    case "PutongCaiGouDanDGM":
                    case "PutongCaiGouDanManager":
                        builder.BeginUpdate("WL物品申购");
                        builder.SetDBNow("审核日期");
                        builder.SetValue("审核员", BaseSystemInfo.UserInfo.Code, null);
                        builder.SetValue("AuditStatus", AuditStatus.AuditReject.ToString(), null);
                        builder.SetWhere("申请单号", objectId);
                        builder.EndUpdate();
                        break;

                    case "YuanFuCaiLiaoShenQingDan":
                        builder.BeginUpdate("WL部门物品申购");
                        builder.SetValue("AuditStatus", AuditStatus.AuditReject.ToString(), null);
                        builder.SetWhere("申购单号", objectId);
                        builder.EndUpdate();
                        break;

                    case "MoJuCaiGouHeTongP":
                    case "MoJuCaiGouHeTongGM":
                        builder.BeginUpdate("GCMJ模具申请");
                        builder.SetValue("AuditStatus", AuditStatus.AuditReject.ToString(), null);
                        builder.SetWhere("申请单号", objectId);
                        builder.EndUpdate();
                        break;
                }
                dbHelper.Close();
            }
            return true;
        }

        public bool OnAuditReject(string[] currentIds, string categoryCode, string auditIdea)
        {
            for (int i = 0; i < currentIds.Length; i++)
            {
                this.OnAuditReject(currentIds[i], categoryCode, auditIdea);
            }
            return true;
        }

        public bool OnAutoAuditPass(string[] currentIds, string categoryCode, string auditIdea)
        {
            for (int i = 0; i < currentIds.Length; i++)
            {
                this.OnAutoAuditPass(currentIds[i], categoryCode, auditIdea);
            }
            return true;
        }

        public bool OnAutoAuditPass(string currentId, string categoryCode, string auditIdea)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string objectId = manager.GetEntity(currentId).ObjectId;
            if (!string.IsNullOrEmpty(objectId))
            {
                IDbHelper dbHelper = new SqlHelper(BaseSystemInfo.BusinessDbConnection);
                dbHelper.Open();
                SQLBuilder builder = new SQLBuilder(dbHelper);
                builder.BeginUpdate("WL物品申购");
                builder.SetDBNow("审核日期");
                builder.SetValue("审核员", BaseSystemInfo.UserInfo.Code, null);
                builder.SetValue("AuditStatus", AuditStatus.AuditPass.ToString(), null);
                builder.SetWhere("申请单号", objectId);
                builder.EndUpdate();
                dbHelper.Close();
            }
            return true;
        }

        public int SendRemindMessage(BaseWorkFlowCurrentEntity workFlowCurrentEntity, AuditStatus auditStatus, string auditIdea, string[] userIds, string[] roleIds)
        {
            userIds = BaseBusinessLogic.Remove(userIds, base.UserInfo.Id);
            BaseMessageEntity messageEntity = new BaseMessageEntity {
                Id = BaseBusinessLogic.NewGuid(),
                FunctionCode = base.GetType().ToString(),
                ObjectId = workFlowCurrentEntity.ObjectId
            };
            if (!string.IsNullOrEmpty(auditIdea))
            {
                auditIdea = " 批示: " + auditIdea;
            }
            messageEntity.Contents = workFlowCurrentEntity.CreateBy + " 发出审批申请： <a title='点击这里，直接查看单据' target='_blank' href='" + this.GetUrl(workFlowCurrentEntity.Id) + "'>" + workFlowCurrentEntity.ObjectFullName + "</a> " + Environment.NewLine + base.UserInfo.RealName + " " + BaseBusinessLogic.GetAuditStatus(auditStatus) + " " + Environment.NewLine + auditIdea;
            messageEntity.IsNew = 1;
            messageEntity.ReadCount = 0;
            messageEntity.Enabled = 1;
            messageEntity.DeletionStateCode = 0;
            BaseMessageManager manager = new BaseMessageManager(base.UserInfo);
            return manager.BatchSend(userIds, null, null, messageEntity, false);
        }

        public void SetUserInfo(BaseUserInfo userInfo)
        {
            base.UserInfo = userInfo;
        }
    }
}

