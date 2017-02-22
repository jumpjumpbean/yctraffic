//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.06.29 版本：2.0 sunmiao 
///		
/// 版本：2.0

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    /// <summary>
    /// 组织机构管理
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class OrganizeService : MarshalByRefObject, IOrganizeService
    {
        private string serviceName = AppMessage.OrganizeService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, BaseOrganizeEntity organizeEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseOrganizeManager manager = new BaseOrganizeManager(helper, userInfo);
                        str = manager.Add(organizeEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_Add, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 依明细情况新增组织机构
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="parentId"></param>
        /// <param name="code"></param>
        /// <param name="fullName"></param>
        /// <param name="categoryId"></param>
        /// <param name="outerPhone"></param>
        /// <param name="innerPhone"></param>
        /// <param name="fax"></param>
        /// <param name="enabled"></param>
        /// <param name="statusCode"></param>
        /// <param name="statusMessage"></param>
        /// <returns></returns>
        public string AddByDetail(BaseUserInfo userInfo, string parentId, string code, string fullName, string categoryId, string outerPhone, string innerPhone, string fax, bool enabled, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseOrganizeManager manager = new BaseOrganizeManager(helper, userInfo);
                        str = manager.AddByDetail(parentId, code, fullName, categoryId, outerPhone, innerPhone, fax, enabled, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_AddByDetail, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int BatchDelete(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).Delete((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_BatchDelete, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 批量移动组织机构到某部门下
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="organizeIds">需要移动的Ids</param>
        /// <param name="parentId">移动的目的部门</param>
        /// <returns></returns>
        public int BatchMoveTo(BaseUserInfo userInfo, string[] organizeIds, string parentId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseOrganizeManager manager = new BaseOrganizeManager(helper, userInfo);
                        for (int i = 0; i < organizeIds.Length; i++)
                        {
                            num += manager.MoveTo(organizeIds[i], parentId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_BatchMoveTo, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 批量储存dataTable中的操作(包括添、删、改)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public int BatchSave(BaseUserInfo userInfo, DataTable dataTable)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).BatchSave(dataTable);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_BatchSave, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 批量重新更新编号
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        public int BatchSetCode(BaseUserInfo userInfo, string[] ids, string[] codes)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).BatchSetCode(ids, codes);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_BatchSetCode, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 批量重新产生排序码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int BatchSetSortCode(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).BatchSetSortCode(ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_BatchSetSortCode, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 物理删除组织机构
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_Delete, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 取得公司列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public DataTable GetCompanyDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable companyDT = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        companyDT = new BaseOrganizeManager(helper, userInfo).GetCompanyDT();
                        companyDT.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        companyDT.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_GetCompanyDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return companyDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 取得部门列表(只取当前组织机构的下一级且类型为Department)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public DataTable GetDepartmentDT(BaseUserInfo userInfo, string organizeId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable departmentDT = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        departmentDT = new BaseOrganizeManager(helper, userInfo).GetDepartmentDT(organizeId);
                        departmentDT.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        departmentDT.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_GetDepartmentDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return departmentDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDT(BaseUserInfo userInfo)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            DataTable table = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseOrganizeManager(helper, userInfo).GetDT(BaseOrganizeEntity.FieldDeletionStateCode, 0, BaseOrganizeEntity.FieldSortCode);
                        table.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        table.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_GetDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 依父节点取得列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public DataTable GetDTByParent(BaseUserInfo userInfo, string parentId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseOrganizeManager(helper, userInfo).GetDT(BaseOrganizeEntity.FieldParentId, parentId, BaseOrganizeEntity.FieldDeletionStateCode, 0, BaseOrganizeEntity.FieldSortCode);
                        table.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        table.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_GetDTByParent, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseOrganizeEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            BaseOrganizeEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseOrganizeManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_GetEntity, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return entity;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 取得内部组织机构
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public DataTable GetInnerOrganizeDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable innerOrganize = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        innerOrganize = new BaseOrganizeManager(helper, userInfo).GetInnerOrganize();
                        innerOrganize.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        innerOrganize.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_GetInnerOrganizeDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return innerOrganize;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 移动组织机构
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="organizeId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int MoveTo(BaseUserInfo userInfo, string organizeId, string parentId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).MoveTo(organizeId, parentId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_MoveTo, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseOrganizeManager(helper, userInfo).Search(string.Empty, searchValue);
                        table.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        table.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_Search, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 批量标示删除标志
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int SetDeleted(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseOrganizeManager(helper, userInfo).SetDeleted((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_SetDeleted, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int Update(BaseUserInfo userInfo, BaseOrganizeEntity organizeEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseOrganizeManager manager = new BaseOrganizeManager(helper, userInfo);
                        num = manager.Update(organizeEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.OrganizeService_Update, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 获得某部门直属的上级公司
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="depID"></param>
        /// <returns></returns>
        public BaseOrganizeEntity GetCompanyByDepID(BaseUserInfo userInfo, string depID)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseOrganizeEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseOrganizeManager manager = new BaseOrganizeManager(helper, userInfo);
                        entity = manager.GetEntity(depID);

                        while (entity.ParentId != null)
                        {
                            if (entity.Category == "Company")
                                break;
                            else
                                entity = manager.GetEntity(entity.ParentId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                }
                finally
                {
                    helper.Close();
                }
            }

            return entity;
        }
    }
}

