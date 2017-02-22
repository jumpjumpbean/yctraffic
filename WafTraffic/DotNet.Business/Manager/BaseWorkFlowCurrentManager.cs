namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseWorkFlowCurrentManager : BaseManager, IBaseManager
    {
        private static readonly object WorkFlowCurrentLock = new object();

        public BaseWorkFlowCurrentManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            }
            base.CurrentTableName = BaseWorkFlowCurrentEntity.TableName;
            base.Identity = false;
        }

        public BaseWorkFlowCurrentManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowCurrentManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseWorkFlowCurrentManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseWorkFlowCurrentManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowCurrentManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity)
        {
            return this.AddEntity(baseWorkFlowCurrentEntity);
        }

        public string Add(BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseWorkFlowCurrentEntity);
        }

        public string AddEntity(BaseWorkFlowCurrentEntity entity)
        {
            string s = string.Empty;
            if (entity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                entity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseWorkFlowCurrentEntity.FieldId);
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldId, entity.Id, null);
            }
            this.SetEntity(sqlBuilder, entity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowCurrentEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowCurrentEntity.FieldModifiedOn);
            sqlBuilder.EndInsert();
            return entity.Id;
        }

        private string AddHistory(BaseWorkFlowCurrentEntity workFlowCurrentEntity)
        {
            BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity = new BaseWorkFlowHistoryEntity {
                CurrentFlowId = workFlowCurrentEntity.Id,
                WorkFlowId = workFlowCurrentEntity.WorkFlowId,
                ActivityId = workFlowCurrentEntity.ActivityId,
                ActivityFullName = workFlowCurrentEntity.ActivityFullName,
                ToUserId = workFlowCurrentEntity.ToUserId,
                ToUserRealName = workFlowCurrentEntity.ToUserRealName,
                ToRoleId = workFlowCurrentEntity.ToRoleId,
                ToRoleRealName = workFlowCurrentEntity.ToRoleRealName,
                ToDepartmentId = workFlowCurrentEntity.ToDepartmentId,
                ToDepartmentName = workFlowCurrentEntity.ToDepartmentName,
                AuditUserId = workFlowCurrentEntity.AuditUserId,
                AuditUserRealName = workFlowCurrentEntity.AuditUserRealName,
                AuditRoleId = workFlowCurrentEntity.AuditRoleId,
                AuditRoleRealName = workFlowCurrentEntity.AuditRoleRealName,
                AuditDepartmentId = workFlowCurrentEntity.AuditDepartmentId,
                AuditDepartmentName = workFlowCurrentEntity.AuditDepartmentName,
                AuditIdea = workFlowCurrentEntity.AuditIdea,
                AuditStatus = workFlowCurrentEntity.AuditStatus,
                SendDate = workFlowCurrentEntity.AuditDate,
                AuditDate = new DateTime?(DateTime.Now),
                Description = workFlowCurrentEntity.Description,
                SortCode = workFlowCurrentEntity.SortCode,
                DeletionStateCode = workFlowCurrentEntity.DeletionStateCode,
                Enabled = workFlowCurrentEntity.Enabled
            };
            BaseWorkFlowHistoryManager manager = new BaseWorkFlowHistoryManager(base.DbHelper, base.UserInfo);
            return manager.AddEntity(baseWorkFlowHistoryEntity);
        }

        private string AddHistory(string currentId)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            return this.AddHistory(workFlowCurrentEntity);
        }

        public int AuditComplete(IWorkFlowManager workFlowManager, string currentId, string auditIdea)
        {
            int num = 0;
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.StepAuditComplete(currentId, auditIdea);
            if (workFlowCurrentEntity.Id != null)
            {
                if (workFlowManager != null)
                {
                    workFlowManager.OnAuditComplete(currentId, workFlowCurrentEntity.CategoryCode, auditIdea);
                    string[] userIds = null;
                    BaseWorkFlowStepManager manager = new BaseWorkFlowStepManager(base.DbHelper, base.UserInfo);
                    userIds = BaseBusinessLogic.Remove(BaseBusinessLogic.Concat(manager.GetIds(BaseWorkFlowStepEntity.FieldCategoryCode, workFlowCurrentEntity.CategoryCode, BaseWorkFlowStepEntity.FieldObjectId, workFlowCurrentEntity.ObjectId, BaseWorkFlowStepEntity.FieldAuditUserId), workFlowCurrentEntity.CreateUserId), base.UserInfo.Id);
                    workFlowManager.SendRemindMessage(workFlowCurrentEntity, AuditStatus.AuditComplete, auditIdea, userIds, null);
                    num = 1;
                }
            }
            else
            {
                base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            }
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return num;
        }

        public int AuditQuash(string currentId, string auditIdea)
        {
            IWorkFlowManager workFlowManager = this.GetWorkFlowManager(currentId);
            return this.AuditQuash(workFlowManager, currentId, auditIdea);
        }

        public int AuditQuash(string[] currentIds, string auditIdea)
        {
            int num = 0;
            for (int i = 0; i < currentIds.Length; i++)
            {
                num += this.AuditQuash(currentIds[i], auditIdea);
            }
            return num;
        }

        public int AuditQuash(IWorkFlowManager workFlowManager, string currentId, string auditIdea)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(currentId))
            {
                try
                {
                    BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
                    if (workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditComplete.ToString()) || workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditQuash.ToString()))
                    {
                        return num;
                    }
                    workFlowCurrentEntity = this.StepAuditQuash(currentId, auditIdea);
                    if (!string.IsNullOrEmpty(workFlowCurrentEntity.Id))
                    {
                        if (workFlowManager != null)
                        {
                            workFlowManager.OnAuditQuash(currentId, workFlowCurrentEntity.CategoryCode, auditIdea);
                            workFlowManager.SendRemindMessage(workFlowCurrentEntity, AuditStatus.AuditQuash, auditIdea, new string[] { workFlowCurrentEntity.CreateUserId }, null);
                            num = 1;
                        }
                    }
                    else
                    {
                        base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
                    }
                    base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
                }
                catch (Exception exception)
                {
                    FileUtil.WriteException(base.UserInfo, exception);
                }
            }
            return num;
        }

        public int AuditQuash(string categoryCode, string objectId, string auditIdea)
        {
            string currentId = this.GetCurrentId(categoryCode, objectId);
            return this.AuditQuash(currentId, auditIdea);
        }

        public int AuditQuash(IWorkFlowManager workFlowManager, string categoryCode, string objectId, string auditIdea)
        {
            string currentId = this.GetCurrentId(categoryCode, objectId);
            return this.AuditQuash(workFlowManager, currentId, auditIdea);
        }

        public int AuditQuash(IWorkFlowManager workFlowManager, string categoryCode, string[] objectIds, string auditIdea)
        {
            int num = 0;
            string currentId = string.Empty;
            for (int i = 0; i < objectIds.Length; i++)
            {
                currentId = this.GetCurrentId(categoryCode, objectIds[i]);
                num += this.AuditQuash(workFlowManager, currentId, auditIdea);
            }
            return num;
        }

        //public int AuditReject(string currentId, string auditIdea, int? historyId = new int?()) //C# 4.0 才支持缺省参数
        public int AuditReject(string currentId, string auditIdea, int? historyId)
        {
            if (historyId==null) historyId = new int?(); // 为避免缺省参数添加的代码

            IWorkFlowManager workFlowManager = this.GetWorkFlowManager(currentId);
            return this.AuditReject(workFlowManager, currentId, auditIdea, historyId);
        }

        //public int AuditReject(string[] currentIds, string auditIdea, int? activityId = new int?())
        public int AuditReject(string[] currentIds, string auditIdea, int? activityId) //C# 4.0 才支持缺省参数
        {
            if (activityId==null) activityId = new int?();// 为避免缺省参数添加的代码

            int num = 0;
            for (int i = 0; i < currentIds.Length; i++)
            {
                num += this.AuditReject(currentIds[i], auditIdea, activityId);
            }
            return num;
        }

        //public int AuditReject(IWorkFlowManager workFlowManager, string currentId, string auditIdea, int? rejectToHistoryId = new int?())//C# 4.0 才支持缺省参数
        public int AuditReject(IWorkFlowManager workFlowManager, string currentId, string auditIdea, int? rejectToHistoryId)
        {
             if (rejectToHistoryId==null) rejectToHistoryId = new int?();// 为避免缺省参数添加的代码

            int num = 0;
            lock (WorkFlowCurrentLock)
            {
                try
                {
                    BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
                    if (!workFlowCurrentEntity.ActivityId.HasValue)
                    {
                        return num;
                    }
                    if (workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditQuash.ToString()))
                    {
                        return num;
                    }
                    int? activityId = null;
                    string createUserId = string.Empty;
                    if (!workFlowCurrentEntity.AuditUserId.ToString().Equals(base.UserInfo.Id) && !workFlowCurrentEntity.ToUserId.ToString().Equals(base.UserInfo.Id))
                    {
                        return num;
                    }
                    if ((workFlowCurrentEntity.AuditUserId.ToString().Equals(base.UserInfo.Id) && workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditReject.ToString())) && !workFlowCurrentEntity.ToUserId.ToString().Equals(base.UserInfo.Id))
                    {
                        return num;
                    }
                    string[] userIds = new string[] { workFlowCurrentEntity.CreateUserId };
                    string str2 = string.Empty;
                    if (rejectToHistoryId.HasValue)
                    {
                        BaseWorkFlowHistoryEntity entity = new BaseWorkFlowHistoryManager(base.DbHelper, base.UserInfo).GetEntity(rejectToHistoryId);
                        createUserId = entity.AuditUserId.ToString();
                        activityId = entity.ActivityId;
                    }
                    else
                    {
                        string str3 = string.Empty;
                        str3 = workFlowCurrentEntity.WorkFlowId.ToString();
                        if (workFlowCurrentEntity.ActivityId.HasValue)
                        {
                            str2 = workFlowCurrentEntity.ActivityId.ToString();
                        }
                        BaseWorkFlowStepManager manager2 = new BaseWorkFlowStepManager(base.DbHelper, base.UserInfo);
                        string[] names = new string[] { BaseWorkFlowStepEntity.FieldCategoryCode, BaseWorkFlowStepEntity.FieldObjectId, BaseWorkFlowStepEntity.FieldWorkFlowId, BaseWorkFlowStepEntity.FieldEnabled, BaseWorkFlowStepEntity.FieldDeletionStateCode };
                        object[] values = new object[] { workFlowCurrentEntity.CategoryCode, workFlowCurrentEntity.ObjectId, str3, 1, 0 };
                        DataTable dataTable = manager2.GetDT(names, values, BaseWorkFlowStepEntity.FieldSortCode);
                        if (dataTable.Rows.Count > 0)
                        {
                            dataTable.Columns.Remove(BaseWorkFlowStepEntity.FieldId);
                            dataTable.Columns[BaseWorkFlowStepEntity.FieldActivityId].ColumnName = BaseWorkFlowStepEntity.FieldId;
                        }
                        else
                        {
                            BaseWorkFlowActivityManager manager3 = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo);
                            names = new string[] { BaseWorkFlowActivityEntity.FieldWorkFlowId, BaseWorkFlowActivityEntity.FieldEnabled, BaseWorkFlowActivityEntity.FieldDeletionStateCode };
                            values = new object[] { str3, 1, 0 };
                            dataTable = manager3.GetDT(names, values, BaseWorkFlowActivityEntity.FieldSortCode);
                        }
                        userIds = BaseBusinessLogic.FieldToArray(dataTable, BaseWorkFlowActivityEntity.FieldAuditUserId);
                        if (dataTable.Rows.Count == 0)
                        {
                            return num;
                        }
                        string previousId = string.Empty;
                        if (!string.IsNullOrEmpty(str2))
                        {
                            previousId = BaseSortLogic.GetPreviousId(dataTable, str2.ToString());
                        }
                        else
                        {
                            previousId = dataTable.Rows[0][BaseWorkFlowActivityEntity.FieldId].ToString();
                        }
                        if (!string.IsNullOrEmpty(previousId))
                        {
                            BaseWorkFlowActivityEntity entity3 = new BaseWorkFlowActivityEntity(BaseBusinessLogic.GetDataRow(dataTable, previousId));
                            createUserId = entity3.AuditUserId.ToString();
                            activityId = new int?(int.Parse(previousId));
                        }
                        else
                        {
                            createUserId = workFlowCurrentEntity.CreateUserId;
                            activityId = null;
                            if (base.UserInfo.Id.Equals(createUserId))
                            {
                                return num;
                            }
                        }
                    }
                    workFlowCurrentEntity = this.StepAuditReject(currentId, auditIdea, createUserId, activityId);
                    if (workFlowCurrentEntity.Id != null)
                    {
                        if (workFlowManager != null)
                        {
                            workFlowManager.OnAuditReject(currentId, workFlowCurrentEntity.CategoryCode, auditIdea);
                            workFlowManager.SendRemindMessage(workFlowCurrentEntity, AuditStatus.AuditReject, auditIdea, userIds, null);
                            num = 1;
                        }
                        num = 1;
                    }
                    else
                    {
                        base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
                    }
                    base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
                }
                catch (Exception exception)
                {
                    FileUtil.WriteException(base.UserInfo, exception);
                }
            }
            return num;
        }

        public int AutoAuditPass(string[] currentIds, string auditIdea)
        {
            int num = 0;
            for (int i = 0; i < currentIds.Length; i++)
            {
                num += this.AutoAuditPass(currentIds[i], auditIdea, null);
            }
            return num;
        }

        //public int AutoAuditPass(string currentId, string auditIdea, string toUserId = null) //C# 4.0 才支持缺省参数
        public int AutoAuditPass(string currentId, string auditIdea, string toUserId)
        {
            IWorkFlowManager workFlowManager = this.GetWorkFlowManager(currentId);
            return this.AutoAuditPass(workFlowManager, currentId, auditIdea, toUserId);
        }

        //public int AutoAuditPass(IWorkFlowManager workFlowManager, string currentId, string auditIdea, string toUserId = null) //C# 4.0 才支持缺省参数
        public int AutoAuditPass(IWorkFlowManager workFlowManager, string currentId, string auditIdea, string toUserId)
        {
            int num = 0;
            lock (WorkFlowCurrentLock)
            {
                BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
                if ((!workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.StartAudit.ToString()) && !workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditPass.ToString())) && (!workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.WaitForAudit.ToString()) && !workFlowCurrentEntity.AuditStatus.Equals(AuditStatus.AuditReject.ToString())))
                {
                    return num;
                }
                if (!workFlowCurrentEntity.ToUserId.ToString().Equals(base.UserInfo.Id) && !workFlowCurrentEntity.ToUserId.ToString().Equals(base.UserInfo.TargetUserId))
                {
                    return num;
                }
                BaseWorkFlowActivityEntity nextWorkFlowActivity = this.GetNextWorkFlowActivity(workFlowCurrentEntity);
                if (nextWorkFlowActivity == null)
                {
                    return this.AuditComplete(workFlowManager, currentId, auditIdea);
                }
                string auditRoleId = string.Empty;
                string auditUserId = string.Empty;
                auditRoleId = nextWorkFlowActivity.AuditRoleId;
                if (string.IsNullOrEmpty(toUserId))
                {
                    auditUserId = nextWorkFlowActivity.AuditUserId;
                }
                else
                {
                    auditUserId = toUserId;
                }
                string targetValue = nextWorkFlowActivity.Id.ToString();
                if (string.IsNullOrEmpty(auditRoleId) && string.IsNullOrEmpty(auditUserId))
                {
                    return num;
                }
                if (!string.IsNullOrEmpty(auditRoleId))
                {
                    //num = this.RoleAuditPass(currentId, auditRoleId, auditIdea, nextWorkFlowActivity.SortCode);
                    num = this.RoleAuditPass(currentId, auditRoleId, auditIdea);
                }
                if (!string.IsNullOrEmpty(auditUserId))
                {
                    if (auditUserId.Equals("Anyone"))
                    {
                        return num;
                    }
                    num = this.UserAuditPass(workFlowManager, currentId, auditUserId, auditIdea, nextWorkFlowActivity.SortCode);
                }
                this.SetProperty(BaseWorkFlowCurrentEntity.FieldId, currentId, BaseWorkFlowCurrentEntity.FieldActivityId, targetValue);
            }
            return num;
        }

        public string AutoStatr(string categoryCode, string categoryFullName, string objectId, string objectFullName, string workFlowCode, string auditIdea)
        {
            string currentId = this.GetCurrentId(categoryCode, objectId);
            IWorkFlowManager workFlowManager = this.GetWorkFlowManager(currentId);
            return this.AutoStatr(workFlowManager, categoryCode, categoryFullName, objectId, objectFullName, workFlowCode, auditIdea, null);
        }

        public string AutoStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string auditIdea, DataTable dtWorkFlowActivity)
        {
            string str = string.Empty;
            if ((dtWorkFlowActivity != null) && (dtWorkFlowActivity.Rows.Count != 0))
            {
                lock (WorkFlowCurrentLock)
                {
                    BaseWorkFlowStepManager manager = new BaseWorkFlowStepManager(base.DbHelper, base.UserInfo);
                    manager.Delete(BaseWorkFlowStepEntity.FieldCategoryCode, categoryCode, BaseWorkFlowStepEntity.FieldObjectId, objectId);
                    BaseWorkFlowStepEntity baseWorkFlowStepEntity = null;
                    foreach (DataRow row in dtWorkFlowActivity.Rows)
                    {
                        baseWorkFlowStepEntity = new BaseWorkFlowStepEntity(row) {
                            ActivityId = baseWorkFlowStepEntity.Id,
                            CategoryCode = categoryCode,
                            ObjectId = objectId
                        };
                        baseWorkFlowStepEntity.Id = null;
                        manager.Add(baseWorkFlowStepEntity);
                    }
                    string sendToUserId = dtWorkFlowActivity.Rows[0][BaseWorkFlowStepEntity.FieldAuditUserId].ToString();
                    string workFlowId = dtWorkFlowActivity.Rows[0][BaseWorkFlowStepEntity.FieldWorkFlowId].ToString();
                    string activityId = dtWorkFlowActivity.Rows[0][BaseWorkFlowStepEntity.FieldId].ToString();
                    str = this.UserStatr(workFlowManager, categoryCode, categoryFullName, objectId, objectFullName, sendToUserId, auditIdea, workFlowId, activityId);
                }
            }
            return str;
        }

        //public string AutoStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string workFlowCode, string auditIdea, string toUserId = null)//C# 4.0 才支持缺省参数
        public string AutoStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string workFlowCode, string auditIdea, string toUserId)
        {
            string str = string.Empty;
            lock (WorkFlowCurrentLock)
            {
                BaseWorkFlowActivityEntity firstActivityEntity = this.GetFirstActivityEntity(workFlowCode, categoryCode);
                if (firstActivityEntity == null)
                {
                    return null;
                }
                if (!string.IsNullOrEmpty(toUserId) || !string.IsNullOrEmpty(firstActivityEntity.AuditUserId))
                {
                    if (string.IsNullOrEmpty(toUserId))
                    {
                        if (firstActivityEntity.AuditUserId.Equals("Anyone"))
                        {
                            return null;
                        }
                        if (!string.IsNullOrEmpty(firstActivityEntity.AuditUserId))
                        {
                            toUserId = firstActivityEntity.AuditUserId;
                        }
                    }
                    str = this.UserStatr(workFlowManager, categoryCode, categoryFullName, objectId, objectFullName, toUserId, auditIdea, firstActivityEntity.WorkFlowId.ToString(), firstActivityEntity.Id.ToString());
                }
                if (!string.IsNullOrEmpty(firstActivityEntity.AuditRoleId))
                {
                    str = this.RoleStatr(workFlowManager, categoryCode, categoryFullName, objectId, objectFullName, firstActivityEntity.AuditRoleId.ToString(), auditIdea, firstActivityEntity.WorkFlowId.ToString(), firstActivityEntity.Id.ToString());
                }
            }
            return str;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseWorkFlowCurrentEntity.FieldId, id);
        }

        public DataTable GetAllWaitForAudit()
        {
            string commandText = " SELECT *    FROM " + BaseWorkFlowCurrentEntity.TableName + "  WHERE (" + BaseWorkFlowCurrentEntity.FieldEnabled + " = ?)     AND (" + BaseWorkFlowCurrentEntity.FieldAuditStatus + "= ? OR " + BaseWorkFlowCurrentEntity.FieldAuditStatus + "= ? OR " + BaseWorkFlowCurrentEntity.FieldAuditStatus + "= ?)";
            string[] targetFileds = new string[4];
            object[] targetValues = new object[4];
            targetFileds[0] = BaseWorkFlowCurrentEntity.FieldEnabled;
            targetValues[0] = 1;
            targetFileds[1] = BaseWorkFlowCurrentEntity.FieldAuditStatus;
            targetValues[1] = AuditStatus.WaitForAudit.ToString();
            targetFileds[2] = BaseWorkFlowCurrentEntity.FieldAuditStatus;
            targetValues[2] = AuditStatus.StartAudit.ToString();
            targetFileds[3] = BaseWorkFlowCurrentEntity.FieldAuditStatus;
            targetValues[3] = AuditStatus.AuditReject.ToString();
            return base.DbHelper.Fill(commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
        }

        //public DataTable GetAuditRecord(string categoryId, string categorybillFullName = null, string searchValue = null)//C# 4.0 才支持缺省参数
        public DataTable GetAuditRecord(string categoryId, string categorybillFullName, string searchValue)
        {
            string commandText = "   SELECT BASE_WORKFLOWCURRENT.*\r\n                                     FROM BASE_WORKFLOWCURRENT\r\n                                    WHERE BASE_WORKFLOWCURRENT.DELETIONSTATECODE = 0\r\n                                          AND (BASE_WORKFLOWCURRENT.Id \r\n                                               IN ( SELECT DISTINCT Base_WorkFlowHistory.CurrentFlowId\r\n                                                      FROM Base_WorkFlowHistory\r\n                                                     WHERE (Base_WorkFlowHistory.DeletionStateCode = 0) \r\n                                                            AND (Base_WorkFlowHistory.AuditUserId = " + base.UserInfo.Id + "  OR Base_WorkFlowHistory.ToUserId = " + base.UserInfo.Id + ") \r\n                                                            AND (Base_WorkFlowHistory.DeletionStateCode = 0)))";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(categoryId))
            {
                TemplateManager manager = new TemplateManager(base.DbHelper, base.UserInfo);
                string str2 = BaseBusinessLogic.FieldToList(manager.Search(string.Empty, categoryId, string.Empty, null, false), BaseFileEntity.FieldId);
                if (!string.IsNullOrEmpty(str2))
                {
                    commandText = commandText + " AND (BASE_WORKFLOWCURRENT.CategoryCode IN (" + str2 + ")) ";
                }
            }
            if (!string.IsNullOrEmpty(categorybillFullName))
            {
                commandText = commandText + " AND (BASE_WORKFLOWCURRENT.CategoryFullName ='" + categorybillFullName + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str3 = commandText;
                string str4 = str3 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName);
                string str5 = str4 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName);
                string str6 = str5 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea);
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName);
                commandText = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName, searchValue));
            }
            commandText = commandText + "  ORDER BY BASE_WORKFLOWCURRENT.SORTCODE ";
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        public string GetCurrentId(string categoryCode, string objectId)
        {
            string[] names = new string[] { BaseWorkFlowCurrentEntity.FieldCategoryCode, BaseWorkFlowCurrentEntity.FieldObjectId, BaseWorkFlowCurrentEntity.FieldDeletionStateCode };
            object[] values = new object[] { categoryCode, objectId, 0 };
            return this.GetId(names, values);
        }

        public BaseWorkFlowCurrentEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseWorkFlowCurrentEntity GetEntity(string id)
        {
            return new BaseWorkFlowCurrentEntity(this.GetDT(BaseWorkFlowCurrentEntity.FieldId, id));
        }

        //public BaseWorkFlowActivityEntity GetFirstActivityEntity(string workFlowCode, string categoryCode = null) //C# 4.0 才支持缺省参数
        public BaseWorkFlowActivityEntity GetFirstActivityEntity(string workFlowCode, string categoryCode)
        {
            BaseWorkFlowActivityEntity entity = null;
            string id = string.Empty;
            if (string.IsNullOrEmpty(workFlowCode))
            {
                TemplateManager manager = new TemplateManager(base.DbHelper, base.UserInfo);
                BaseNewsEntity entity2 = new BaseNewsEntity(manager.GetDT(BaseNewsEntity.FieldDeletionStateCode, 0, BaseNewsEntity.FieldCode, categoryCode));
                if (!string.IsNullOrEmpty(entity2.Id))
                {
                    workFlowCode = base.UserInfo.Id + "_" + entity2.Id;
                }
            }
            if (string.IsNullOrEmpty(workFlowCode))
            {
                return null;
            }
            BaseWorkFlowProcessManager manager2 = new BaseWorkFlowProcessManager(base.DbHelper, base.UserInfo);
            string[] names = new string[] { BaseWorkFlowProcessEntity.FieldCode, BaseWorkFlowProcessEntity.FieldEnabled, BaseWorkFlowProcessEntity.FieldDeletionStateCode };
            object[] values = new object[] { workFlowCode, 1, 0 };
            id = manager2.GetId(names, values);
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            DataTable table2 = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo).GetDT(new string[] { BaseWorkFlowActivityEntity.FieldWorkFlowId, BaseWorkFlowActivityEntity.FieldDeletionStateCode, BaseWorkFlowActivityEntity.FieldEnabled }, new object[] { id.ToString(), 0, 1 }, BaseWorkFlowActivityEntity.FieldSortCode);
            if (table2.Rows.Count == 0)
            {
                return null;
            }
            entity = new BaseWorkFlowActivityEntity(table2.Rows[0]);
            if ((entity.AuditUserId == null) && (entity.AuditRoleId == null))
            {
                return null;
            }
            return entity;
        }

        //public DataTable GetMonitorDT(string categoryCode = null, string searchValue = null) //C# 4.0 才支持缺省参数
        public DataTable GetMonitorDT(string categoryCode, string searchValue)
        {
            string commandText = "   SELECT BASE_WORKFLOWCURRENT.*\r\n                                     FROM BASE_WORKFLOWCURRENT\r\n                                    WHERE BASE_WORKFLOWCURRENT.DELETIONSTATECODE = 0\r\n                                          AND (BASE_WORKFLOWCURRENT.ENABLED = 0)";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(categoryCode))
            {
                commandText = commandText + " AND (BASE_WORKFLOWCURRENT.CategoryCode ='" + categoryCode + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str2 = commandText;
                string str3 = str2 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName);
                string str4 = str3 + " OR " + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldCreateBy);
                string str5 = str4 + " OR " + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName);
                string str6 = str5 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea);
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName);
                commandText = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldCreateBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName, searchValue));
            }
            commandText = commandText + " ORDER BY BASE_WORKFLOWCURRENT.SORTCODE ";
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        //public DataTable GetMonitorPagedDT(int pageSize, int currentPage, out int recordCount, string categoryCode = null, string searchValue = null) //C# 4.0 才支持缺省参数
        public DataTable GetMonitorPagedDT(int pageSize, int currentPage, out int recordCount, string categoryCode, string searchValue)
        {
            string str3 = string.Empty;
            string sqlQuery = str3 + "SELECT * FROM " + BaseWorkFlowCurrentEntity.TableName + " WHERE " + BaseWorkFlowCurrentEntity.FieldDeletionStateCode + " = 0  AND " + BaseWorkFlowCurrentEntity.FieldEnabled + " = 0 ";
            if (!string.IsNullOrEmpty(categoryCode))
            {
                string str4 = sqlQuery;
                sqlQuery = str4 + " AND " + BaseWorkFlowCurrentEntity.FieldCategoryCode + "= '" + categoryCode + "') ";
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str5 = sqlQuery;
                string str6 = str5 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE '" + searchValue + "'";
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldCreateBy + " LIKE '" + searchValue + "'";
                string str8 = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE '" + searchValue + "'";
                string str9 = str8 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE '" + searchValue + "'";
                string str10 = str9 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE '" + searchValue + "'";
                sqlQuery = str10 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE '" + searchValue + "')";
            }
            string fieldSortCode = BaseWorkFlowCurrentEntity.FieldSortCode;
            return this.GetDTByPage(out recordCount, currentPage, pageSize, sqlQuery, fieldSortCode);
        }

        public BaseWorkFlowActivityEntity GetNextWorkFlowActivity(BaseWorkFlowCurrentEntity workFlowCurrentEntity)
        {
            BaseWorkFlowActivityEntity entity = null;
            DataTable dataTable = null;
            string str = workFlowCurrentEntity.WorkFlowId.ToString();
            string[] names = null;
            object[] values = null;
            BaseWorkFlowStepManager manager = new BaseWorkFlowStepManager(base.DbHelper, base.UserInfo);
            names = new string[] { BaseWorkFlowStepEntity.FieldCategoryCode, BaseWorkFlowStepEntity.FieldObjectId, BaseWorkFlowStepEntity.FieldWorkFlowId, BaseWorkFlowStepEntity.FieldEnabled, BaseWorkFlowStepEntity.FieldDeletionStateCode };
            values = new object[] { workFlowCurrentEntity.CategoryCode, workFlowCurrentEntity.ObjectId, str, 1, 0 };
            dataTable = manager.GetDT(names, values, BaseWorkFlowStepEntity.FieldSortCode);
            if (dataTable.Rows.Count > 0)
            {
                dataTable.Columns.Remove(BaseWorkFlowStepEntity.FieldId);
                dataTable.Columns[BaseWorkFlowStepEntity.FieldActivityId].ColumnName = BaseWorkFlowStepEntity.FieldId;
            }
            else
            {
                BaseWorkFlowActivityManager manager2 = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo);
                names = new string[] { BaseWorkFlowActivityEntity.FieldWorkFlowId, BaseWorkFlowActivityEntity.FieldEnabled, BaseWorkFlowActivityEntity.FieldDeletionStateCode };
                values = new object[] { str, 1, 0 };
                dataTable = manager2.GetDT(names, values, BaseWorkFlowActivityEntity.FieldSortCode);
            }
            string str2 = string.Empty;
            if (workFlowCurrentEntity.ActivityId.HasValue)
            {
                str2 = workFlowCurrentEntity.ActivityId.ToString();
            }
            if (dataTable.Rows.Count != 0)
            {
                string nextId = string.Empty;
                if (!string.IsNullOrEmpty(str2))
                {
                    nextId = BaseSortLogic.GetNextId(dataTable, str2.ToString());
                }
                else
                {
                    nextId = dataTable.Rows[0][BaseWorkFlowActivityEntity.FieldId].ToString();
                }
                if (!string.IsNullOrEmpty(nextId))
                {
                    entity = new BaseWorkFlowActivityEntity(BaseBusinessLogic.GetDataRow(dataTable, nextId));
                }
            }
            return entity;
        }

        //public DataTable GetPagedDT(int pageSize, int currentPage, out int recordCount, string categoryCode = null, string searchValue = null) //C# 4.0 才支持缺省参数
        public DataTable GetPagedDT(int pageSize, int currentPage, out int recordCount, string categoryCode, string searchValue) 
        {
            string str3 = string.Empty;
            string sqlQuery = str3 + "SELECT * FROM " + BaseWorkFlowCurrentEntity.TableName + " WHERE " + BaseWorkFlowCurrentEntity.FieldDeletionStateCode + " = 0 ";
            if (!string.IsNullOrEmpty(categoryCode))
            {
                string str4 = sqlQuery;
                sqlQuery = str4 + " AND " + BaseWorkFlowCurrentEntity.FieldCategoryCode + "= '" + categoryCode + "'";
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str5 = sqlQuery;
                string str6 = str5 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE '" + searchValue + "'";
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldCreateBy + " LIKE '" + searchValue + "'";
                string str8 = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE '" + searchValue + "'";
                string str9 = str8 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE '" + searchValue + "'";
                string str10 = str9 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE '" + searchValue + "'";
                sqlQuery = str10 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE '" + searchValue + "')";
            }
            string fieldSortCode = BaseWorkFlowCurrentEntity.FieldSortCode;
            return this.GetDTByPage(out recordCount, currentPage, pageSize, sqlQuery, fieldSortCode);
        }

        public DataTable GetRoleWaitForAudit(string roleId, string categoryCode)
        {
            string commandText = " SELECT *    FROM " + BaseWorkFlowCurrentEntity.TableName + "  WHERE (" + BaseWorkFlowCurrentEntity.FieldDeletionStateCode + " = 0) AND (" + BaseWorkFlowCurrentEntity.FieldEnabled + " = 1) ";
            if (!string.IsNullOrEmpty(roleId))
            {
                string str2 = commandText;
                commandText = str2 + "    AND (" + BaseWorkFlowCurrentEntity.FieldToRoleId + " = " + roleId + ") ";
            }
            if (!string.IsNullOrEmpty(roleId))
            {
                string str3 = commandText;
                commandText = str3 + "   AND (" + BaseWorkFlowCurrentEntity.FieldCategoryCode + " = '" + categoryCode + "') ";
            }
            commandText = commandText + " ORDER BY " + BaseWorkFlowCurrentEntity.FieldSendDate;
            return base.DbHelper.Fill(commandText);
        }

        //public DataTable GetShareBillDT(string categoryFullName = null, string searchValue = null) //C# 4.0 才支持缺省参数
        public DataTable GetShareBillDT(string categoryFullName, string searchValue)
        {
            string commandText = "   SELECT BASE_WORKFLOWCURRENT.*\r\n                                     FROM BASE_WORKFLOWCURRENT, BASE_WORKFLOWACTIVITY\r\n                                    WHERE BASE_WORKFLOWCURRENT.DELETIONSTATECODE = 0\r\n                                          AND (BASE_WORKFLOWACTIVITY.AUDITUSERID = '" + base.UserInfo.Id + "') \r\n                                          AND (BASE_WORKFLOWACTIVITY.DELETIONSTATECODE = 0) \r\n                                          AND (BASE_WORKFLOWACTIVITY.ENABLED = 0)\r\n                                          AND (BASE_WORKFLOWACTIVITY.WORKFLOWID = BASE_WORKFLOWCURRENT.WORKFLOWID) ";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(categoryFullName))
            {
                commandText = commandText + " AND (BASE_WORKFLOWCURRENT.CategoryFullName ='" + categoryFullName + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str2 = commandText;
                string str3 = str2 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName);
                string str4 = str3 + " OR " + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldCreateBy);
                string str5 = str4 + " OR " + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName);
                string str6 = str5 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea);
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName);
                commandText = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldCreateBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName, searchValue));
            }
            commandText = commandText + " ORDER BY BASE_WORKFLOWCURRENT.CREATEON DESC ";
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        public DataTable GetSuperAudit(string categoryCode, string searchValue)
        {
            string commandText = "   SELECT BASE_WORKFLOWCURRENT.*\r\n                                     FROM BASE_WORKFLOWCURRENT, BASE_WORKFLOWACTIVITY\r\n                                    WHERE BASE_WORKFLOWCURRENT.DELETIONSTATECODE = 0\r\n                                          AND (BASE_WORKFLOWACTIVITY.AUDITUSERID = '" + base.UserInfo.Id + "') \r\n                                          AND (BASE_WORKFLOWACTIVITY.DELETIONSTATECODE = 0) \r\n                                          AND (BASE_WORKFLOWACTIVITY.ENABLED = 1)\r\n                                          AND (BASE_WORKFLOWACTIVITY.WORKFLOWID = BASE_WORKFLOWCURRENT.WORKFLOWID) \r\n                                          AND (BASE_WORKFLOWACTIVITY.SORTCODE > BASE_WORKFLOWCURRENT.SORTCODE)";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(categoryCode))
            {
                commandText = commandText + " AND (BASE_WORKFLOWCURRENT.CategoryCode ='" + categoryCode + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str2 = commandText;
                string str3 = str2 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName);
                string str4 = str3 + " OR " + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldCreateBy);
                string str5 = str4 + " OR " + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName);
                string str6 = str5 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea);
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName);
                commandText = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldCreateBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName, searchValue));
            }
            commandText = commandText + " ORDER BY BASE_WORKFLOWCURRENT.SORTCODE ";
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        //public DataTable GetWaitForAudit(string userId = null, string categoryFullName = null, string searchValue = null) //C# 4.0 才支持缺省参数
        public DataTable GetWaitForAudit(string userId, string categoryFullName, string searchValue)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = base.UserInfo.Id;
            }
            string commandText = (" SELECT *    FROM " + BaseWorkFlowCurrentEntity.TableName + "  WHERE (" + BaseWorkFlowCurrentEntity.FieldDeletionStateCode + " = 0)     AND (" + BaseWorkFlowCurrentEntity.FieldEnabled + " = 0)     AND (" + BaseWorkFlowCurrentEntity.FieldToUserId + "=" + userId + " ") + " ) ";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(categoryFullName))
            {
                string str2 = commandText;
                commandText = str2 + " AND (" + BaseWorkFlowCurrentEntity.TableName + "." + BaseWorkFlowCurrentEntity.FieldCategoryFullName + " ='" + categoryFullName + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim();
                string str3 = commandText;
                string str4 = str3 + " AND (" + BaseWorkFlowCurrentEntity.FieldObjectFullName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName);
                string str5 = str4 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName);
                string str6 = str5 + " OR " + BaseWorkFlowCurrentEntity.FieldAuditIdea + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea);
                string str7 = str6 + " OR " + BaseWorkFlowCurrentEntity.FieldToDepartmentName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName);
                commandText = str7 + " OR " + BaseWorkFlowCurrentEntity.FieldToUserRealName + " LIKE " + base.DbHelper.GetParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldObjectFullName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditUserRealName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldAuditIdea, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToDepartmentName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseWorkFlowCurrentEntity.FieldToUserRealName, searchValue));
            }
            commandText = commandText + " ORDER BY " + BaseWorkFlowCurrentEntity.FieldSendDate;
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        public IWorkFlowManager GetWorkFlowManager(string currentId)
        {
            IWorkFlowManager manager = null;
            manager = (IWorkFlowManager) Activator.CreateInstance(Type.GetType(this.GetEntity(currentId).CallBack, true));
            manager.SetUserInfo(base.UserInfo);
            return manager;
        }

        public int ReplaceUser(string oldUserId, string newUserId)
        {
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(newUserId);
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(base.CurrentTableName);
            builder.SetValue(BaseWorkFlowCurrentEntity.FieldToUserId, entity.Id, null);
            builder.SetValue(BaseWorkFlowCurrentEntity.FieldToUserRealName, entity.RealName, null);
            builder.SetValue(BaseWorkFlowCurrentEntity.FieldToDepartmentId, entity.DepartmentId, null);
            builder.SetValue(BaseWorkFlowCurrentEntity.FieldToDepartmentName, entity.DepartmentName, null);
            builder.SetWhere(BaseWorkFlowCurrentEntity.FieldToUserId, oldUserId, "OldUserId", " AND ");
            return builder.EndUpdate();
        }

        //public int RoleAuditPass(string currentId, string sendToRoleId, string auditIdea, int? sortCode = new int?()) //C# 4.0 才支持缺省参数
        public int RoleAuditPass(string currentId, string sendToRoleId, string auditIdea) //sortCode未使用，可以有问题
        {
            int num = 0;
            num = this.StepRoleAuditPass(currentId, sendToRoleId, auditIdea);
            if (num == 0)
            {
                base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            }
            this.GetEntity(currentId);
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return num;
        }

        public string RoleStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToRoleId, string auditIdea, string workFlowId, string activityId)
        {
            string currentId = this.GetCurrentId(categoryCode, objectId);
            if (currentId.Length > 0)
            {
                this.UpdataRoleStatr(workFlowManager, currentId, categoryCode, categoryFullName, objectId, objectFullName, sendToRoleId, auditIdea, workFlowId, activityId);
                return currentId;
            }
            currentId = this.StepRoleStatr(workFlowManager, categoryCode, categoryFullName, objectId, objectFullName, sendToRoleId, auditIdea, workFlowId, activityId);
            base.ReturnStatusCode = StatusCode.OK.ToString();
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return currentId;
        }

        public int SendRemindMessage(string id, AuditStatus auditStatus, string auditIdea, string[] userIds, string[] roleIds)
        {
            BaseMessageEntity messageEntity = new BaseMessageEntity {
                Id = BaseBusinessLogic.NewGuid(),
                FunctionCode = base.GetType().ToString(),
                ObjectId = id,
                IsNew = 1,
                ReadCount = 0,
                Enabled = 1,
                DeletionStateCode = 0
            };
            BaseMessageManager manager = new BaseMessageManager(base.UserInfo);
            return manager.BatchSend(userIds, null, null, messageEntity, false);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity)
        {
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldCallBack, baseWorkFlowCurrentEntity.CallBack, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldCategoryCode, baseWorkFlowCurrentEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldCategoryFullName, baseWorkFlowCurrentEntity.CategoryFullName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldObjectId, baseWorkFlowCurrentEntity.ObjectId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldObjectFullName, baseWorkFlowCurrentEntity.ObjectFullName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldWorkFlowId, baseWorkFlowCurrentEntity.WorkFlowId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldActivityId, baseWorkFlowCurrentEntity.ActivityId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldActivityFullName, baseWorkFlowCurrentEntity.ActivityFullName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldToDepartmentId, baseWorkFlowCurrentEntity.ToDepartmentId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldToDepartmentName, baseWorkFlowCurrentEntity.ToDepartmentName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldToUserId, baseWorkFlowCurrentEntity.ToUserId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldToUserRealName, baseWorkFlowCurrentEntity.ToUserRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldToRoleId, baseWorkFlowCurrentEntity.ToRoleId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldToRoleRealName, baseWorkFlowCurrentEntity.ToRoleRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditDepartmentId, baseWorkFlowCurrentEntity.AuditDepartmentId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditDepartmentName, baseWorkFlowCurrentEntity.AuditDepartmentName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditUserId, baseWorkFlowCurrentEntity.AuditUserId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditUserCode, baseWorkFlowCurrentEntity.AuditUserCode, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditUserRealName, baseWorkFlowCurrentEntity.AuditUserRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditRoleId, baseWorkFlowCurrentEntity.AuditRoleId, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditRoleRealName, baseWorkFlowCurrentEntity.AuditRoleRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldSendDate, baseWorkFlowCurrentEntity.SendDate, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditDate, baseWorkFlowCurrentEntity.AuditDate, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditIdea, baseWorkFlowCurrentEntity.AuditIdea, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldAuditStatus, baseWorkFlowCurrentEntity.AuditStatus, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldSortCode, baseWorkFlowCurrentEntity.SortCode, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldEnabled, baseWorkFlowCurrentEntity.Enabled, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldDeletionStateCode, baseWorkFlowCurrentEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldDescription, baseWorkFlowCurrentEntity.Description, null);
        }

        public string StartAudit(int workFlowId, string categoryCode, string categoryFullName, string objectId, string objectFullName)
        {
            string currentId = string.Empty;
            currentId = this.GetCurrentId(categoryCode, objectId);
            if (currentId.Length <= 0)
            {
                currentId = this.StepStartAudit(workFlowId, categoryCode, categoryFullName, objectId, objectFullName);
                this.AddHistory(currentId);
            }
            return currentId;
        }

        private BaseWorkFlowCurrentEntity StepAuditComplete(string currentId, string auditIdea)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.AuditDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.AuditDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditIdea = auditIdea;
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            workFlowCurrentEntity.Enabled = 1;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditComplete.ToString();
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.ToDepartmentId = null;
            workFlowCurrentEntity.ToDepartmentName = null;
            workFlowCurrentEntity.ToUserId = null;
            workFlowCurrentEntity.ToUserRealName = null;
            workFlowCurrentEntity.ToRoleId = null;
            workFlowCurrentEntity.ToRoleRealName = null;
            this.UpdateEntity(workFlowCurrentEntity);
            return workFlowCurrentEntity;
        }

        private BaseWorkFlowCurrentEntity StepAuditQuash(string currentId, string auditIdea)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.ToDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.ToUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.ToUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.ToDepartmentId = null;
            workFlowCurrentEntity.ToDepartmentName = string.Empty;
            workFlowCurrentEntity.ToRoleId = null;
            workFlowCurrentEntity.ToRoleRealName = string.Empty;
            workFlowCurrentEntity.ToUserId = null;
            workFlowCurrentEntity.ToUserRealName = string.Empty;
            workFlowCurrentEntity.AuditIdea = auditIdea;
            workFlowCurrentEntity.SendDate = new DateTime?(DateTime.Now);
            workFlowCurrentEntity.AuditDate = null;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditQuash.ToString();
            workFlowCurrentEntity.Enabled = 1;
            workFlowCurrentEntity.DeletionStateCode = 0;
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.DeletionStateCode = 1;
            this.UpdateEntity(workFlowCurrentEntity);
            return workFlowCurrentEntity;
        }

        //private BaseWorkFlowCurrentEntity StepAuditReject(string currentId, string auditIdea, string sendToUserId, int? activityId = new int?()) //C# 4.0 才支持缺省参数
        private BaseWorkFlowCurrentEntity StepAuditReject(string currentId, string auditIdea, string sendToUserId, int? activityId)
        {
            if(activityId==null) activityId = new int?();

            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.ToUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.ToUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.ToDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditReject.ToString();
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            this.AddHistory(workFlowCurrentEntity);
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(sendToUserId);
            workFlowCurrentEntity.ActivityId = activityId;
            if (activityId.HasValue)
            {
                BaseWorkFlowActivityEntity entity3 = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo).GetEntity(activityId);
                workFlowCurrentEntity.SortCode = entity3.SortCode;
            }
            workFlowCurrentEntity.ToUserId = entity.Id;
            workFlowCurrentEntity.ToUserRealName = entity.RealName;
            workFlowCurrentEntity.ToDepartmentId = entity.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = entity.DepartmentName;
            workFlowCurrentEntity.ToRoleId = null;
            workFlowCurrentEntity.ToRoleRealName = string.Empty;
            workFlowCurrentEntity.AuditRoleId = null;
            workFlowCurrentEntity.AuditRoleRealName = string.Empty;
            workFlowCurrentEntity.AuditDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.AuditDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditUserCode = base.UserInfo.Code;
            workFlowCurrentEntity.AuditIdea = auditIdea;
            if (activityId.HasValue)
            {
                BaseWorkFlowActivityManager manager3 = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo);
                workFlowCurrentEntity.SortCode = manager3.GetEntity(activityId).SortCode;
            }
            workFlowCurrentEntity.SendDate = new DateTime?(DateTime.Now);
            workFlowCurrentEntity.AuditDate = null;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditReject.ToString();
            workFlowCurrentEntity.Enabled = 0;
            this.UpdateEntity(workFlowCurrentEntity);
            return workFlowCurrentEntity;
        }

        private int StepRoleAuditPass(string currentId, string sendToRoleId, string auditIdea)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.ToDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.ToUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.ToUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditPass.ToString();
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.AuditIdea = auditIdea;
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            workFlowCurrentEntity.AuditDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.AuditDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            BaseRoleEntity entity = new BaseRoleManager(base.UserInfo).GetEntity(sendToRoleId);
            workFlowCurrentEntity.ToRoleId = new int?(int.Parse(sendToRoleId));
            workFlowCurrentEntity.ToRoleRealName = entity.RealName;
            workFlowCurrentEntity.ToUserId = null;
            workFlowCurrentEntity.ToUserRealName = null;
            if (entity.OrganizeId.HasValue)
            {
                BaseOrganizeEntity entity3 = new BaseOrganizeManager(base.UserInfo).GetEntity(entity.OrganizeId);
                workFlowCurrentEntity.ToDepartmentId = entity.OrganizeId;
                workFlowCurrentEntity.ToDepartmentName = entity3.FullName;
            }
            workFlowCurrentEntity.AuditStatus = AuditStatus.WaitForAudit.ToString();
            workFlowCurrentEntity.Enabled = 1;
            workFlowCurrentEntity.DeletionStateCode = 0;
            return this.UpdateEntity(workFlowCurrentEntity);
        }

        private string StepRoleStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToRoleId, string auditIdea, string workFlowId, string activityId)
        {
            BaseWorkFlowCurrentEntity entity = new BaseWorkFlowCurrentEntity {
                WorkFlowId = new int?(int.Parse(workFlowId)),
                ActivityId = new int?(int.Parse(activityId)),
                CategoryCode = categoryCode,
                CategoryFullName = categoryFullName,
                ObjectId = objectId,
                ObjectFullName = objectFullName,
                AuditStatus = AuditStatus.StartAudit.ToString(),
                AuditUserId = new int?(int.Parse(base.UserInfo.Id)),
                AuditUserRealName = base.UserInfo.RealName,
                AuditIdea = auditIdea,
                AuditDate = new DateTime?(DateTime.Now),
                SendDate = new DateTime?(DateTime.Now)
            };
            BaseRoleEntity entity2 = new BaseRoleManager(base.UserInfo).GetEntity(sendToRoleId);
            entity.ToUserId = null;
            entity.ToUserRealName = null;
            entity.ToRoleId = new int?(int.Parse(sendToRoleId));
            entity.ToRoleRealName = entity2.RealName;
            if (entity2.OrganizeId.HasValue)
            {
                BaseOrganizeEntity entity3 = new BaseOrganizeManager(base.UserInfo).GetEntity(entity2.OrganizeId.ToString());
                entity.ToDepartmentId = entity2.OrganizeId;
                entity.ToDepartmentName = entity3.FullName;
            }
            entity.Enabled = 1;
            entity.DeletionStateCode = 0;
            return this.AddEntity(entity);
        }

        private int StepRoleStatrUpdata(IWorkFlowManager workFlowManager, string currentId, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToRoleId, string auditIdea, string workFlowId, string activityId)
        {
            BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity = this.GetEntity(currentId);
            baseWorkFlowCurrentEntity.CategoryCode = categoryCode;
            baseWorkFlowCurrentEntity.CategoryFullName = categoryFullName;
            baseWorkFlowCurrentEntity.ObjectId = objectId;
            baseWorkFlowCurrentEntity.ObjectFullName = objectFullName;
            baseWorkFlowCurrentEntity.WorkFlowId = new int?(int.Parse(workFlowId));
            baseWorkFlowCurrentEntity.ActivityId = new int?(int.Parse(activityId));
            baseWorkFlowCurrentEntity.AuditStatus = AuditStatus.StartAudit.ToString();
            BaseRoleEntity entity = new BaseRoleManager(base.UserInfo).GetEntity(sendToRoleId);
            baseWorkFlowCurrentEntity.ToRoleId = new int?(int.Parse(sendToRoleId));
            baseWorkFlowCurrentEntity.ToRoleRealName = entity.RealName;
            if (entity.OrganizeId.HasValue)
            {
                BaseOrganizeEntity entity3 = new BaseOrganizeManager(base.UserInfo).GetEntity(entity.OrganizeId.ToString());
                baseWorkFlowCurrentEntity.ToDepartmentId = entity.OrganizeId;
                baseWorkFlowCurrentEntity.ToDepartmentName = entity3.FullName;
            }
            baseWorkFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            baseWorkFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            baseWorkFlowCurrentEntity.SendDate = new DateTime?(DateTime.Now);
            baseWorkFlowCurrentEntity.AuditIdea = auditIdea;
            baseWorkFlowCurrentEntity.Enabled = 1;
            baseWorkFlowCurrentEntity.DeletionStateCode = 0;
            return this.UpdateEntity(baseWorkFlowCurrentEntity);
        }

        private string StepStartAudit(int workFlowId, string categoryCode, string categoryFullName, string objectId, string objectFullName)
        {
            BaseWorkFlowCurrentEntity entity = new BaseWorkFlowCurrentEntity {
                ActivityId = 1,
                WorkFlowId = new int?(workFlowId),
                CategoryCode = categoryCode,
                CategoryFullName = categoryFullName,
                ObjectId = objectId,
                ObjectFullName = objectFullName,
                AuditUserId = new int?(int.Parse(base.UserInfo.Id)),
                AuditUserRealName = base.UserInfo.RealName,
                AuditDepartmentId = base.UserInfo.DepartmentId,
                AuditDepartmentName = base.UserInfo.DepartmentName,
                Enabled = 1,
                DeletionStateCode = 0,
                AuditStatus = AuditStatus.StartAudit.ToString()
            };
            return this.AddEntity(entity);
        }

        private int StepTransmitRole(string currentId, string sendToRoleId, string auditIdea)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.ToDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.ToUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.ToUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditStatus = AuditStatus.Transmit.ToString();
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.AuditIdea = auditIdea;
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            workFlowCurrentEntity.AuditDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.AuditDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            BaseRoleEntity entity = new BaseRoleManager(base.UserInfo).GetEntity(sendToRoleId);
            workFlowCurrentEntity.ToRoleId = new int?(int.Parse(sendToRoleId));
            workFlowCurrentEntity.ToRoleRealName = entity.RealName;
            workFlowCurrentEntity.ToUserId = null;
            workFlowCurrentEntity.ToUserRealName = null;
            if (entity.OrganizeId.HasValue)
            {
                BaseOrganizeEntity entity3 = new BaseOrganizeManager(base.UserInfo).GetEntity(entity.OrganizeId);
                workFlowCurrentEntity.ToDepartmentId = entity.OrganizeId;
                workFlowCurrentEntity.ToDepartmentName = entity3.FullName;
            }
            workFlowCurrentEntity.AuditStatus = AuditStatus.WaitForAudit.ToString();
            workFlowCurrentEntity.Enabled = 1;
            workFlowCurrentEntity.DeletionStateCode = 0;
            return this.UpdateEntity(workFlowCurrentEntity);
        }

        private int StepTransmitUser(string currentId, string sendToUserId, string auditIdea)
        {
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.ToDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.ToUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.ToUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditStatus = AuditStatus.Transmit.ToString();
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.AuditDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.AuditDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            workFlowCurrentEntity.AuditIdea = auditIdea;
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(sendToUserId);
            workFlowCurrentEntity.ToRoleId = null;
            workFlowCurrentEntity.ToRoleRealName = null;
            workFlowCurrentEntity.ToDepartmentId = null;
            workFlowCurrentEntity.ToDepartmentName = null;
            workFlowCurrentEntity.ToUserId = new int?(int.Parse(sendToUserId));
            workFlowCurrentEntity.ToUserRealName = entity.RealName;
            if (entity.DepartmentId > 0)
            {
                BaseOrganizeEntity entity3 = new BaseOrganizeManager(base.UserInfo).GetEntity(entity.DepartmentId);
                workFlowCurrentEntity.ToDepartmentId = entity.DepartmentId;
                workFlowCurrentEntity.ToDepartmentName = entity3.FullName;
            }
            workFlowCurrentEntity.AuditStatus = AuditStatus.WaitForAudit.ToString();
            workFlowCurrentEntity.Enabled = 1;
            workFlowCurrentEntity.DeletionStateCode = 0;
            return this.UpdateEntity(workFlowCurrentEntity);
        }

        //private int StepUserAuditPass(string currentId, string sendToUserId, string auditIdea, int? sortCode = new int?()) //C# 4.0 才支持缺省参数
        private int StepUserAuditPass(string currentId, string sendToUserId, string auditIdea, int? sortCode)
        {
            if (sortCode ==null) sortCode = new int?();

            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowCurrentEntity.AuditIdea = auditIdea;
            workFlowCurrentEntity.AuditDepartmentId = base.UserInfo.DepartmentId;
            workFlowCurrentEntity.AuditDepartmentName = base.UserInfo.DepartmentName;
            workFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            workFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            workFlowCurrentEntity.AuditStatus = AuditStatus.AuditPass.ToString();
            this.AddHistory(workFlowCurrentEntity);
            workFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(sendToUserId);
            workFlowCurrentEntity.ToRoleId = null;
            workFlowCurrentEntity.ToRoleRealName = null;
            workFlowCurrentEntity.ToDepartmentId = entity.DepartmentId;
            workFlowCurrentEntity.ToDepartmentName = entity.DepartmentName;
            workFlowCurrentEntity.ToUserId = entity.Id;
            workFlowCurrentEntity.ToUserRealName = entity.RealName;
            workFlowCurrentEntity.SortCode = sortCode;
            return this.UpdateEntity(workFlowCurrentEntity);
        }

        private string StepUserStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToUserId, string auditIdea, string workFlowId, string activityId)
        {
            BaseWorkFlowCurrentEntity entity = new BaseWorkFlowCurrentEntity {
                CallBack = workFlowManager.GetType().ToString(),
                WorkFlowId = new int?(int.Parse(workFlowId)),
                ActivityId = new int?(int.Parse(activityId))
            };
            BaseWorkFlowActivityManager manager = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo);
            entity.SortCode = manager.GetEntity(activityId).SortCode;
            entity.CategoryCode = categoryCode;
            entity.CategoryFullName = categoryFullName;
            entity.ObjectId = objectId;
            entity.ObjectFullName = objectFullName;
            entity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            entity.AuditUserCode = base.UserInfo.Code;
            entity.AuditUserRealName = base.UserInfo.RealName;
            entity.AuditDepartmentId = base.UserInfo.DepartmentId;
            entity.AuditDepartmentName = base.UserInfo.DepartmentName;
            entity.AuditIdea = auditIdea;
            entity.AuditStatus = AuditStatus.StartAudit.ToString();
            entity.SendDate = new DateTime?(DateTime.Now);
            entity.AuditDate = new DateTime?(DateTime.Now);
            BaseUserEntity entity2 = new BaseUserManager(base.UserInfo).GetEntity(sendToUserId);
            entity.ToUserId = new int?(int.Parse(sendToUserId));
            entity.ToUserRealName = entity2.RealName;
            entity.ToDepartmentId = entity2.DepartmentId;
            entity.ToDepartmentName = entity2.DepartmentName;
            entity.Enabled = 0;
            entity.DeletionStateCode = 0;
            return this.AddEntity(entity);
        }

        public int TransmitRole(string currentId, string sendToRoleId, string auditIdea)
        {
            int num = 0;
            num = this.StepTransmitRole(currentId, sendToRoleId, auditIdea);
            if (num == 0)
            {
                base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            }
            this.GetEntity(currentId);
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return num;
        }

        public int TransmitUser(string currentId, string sendToUserId, string auditIdea)
        {
            int num = 0;
            num = this.StepTransmitUser(currentId, sendToUserId, auditIdea);
            if (num == 0)
            {
                base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            }
            this.GetEntity(currentId);
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return num;
        }

        public int UpdataRoleStatr(IWorkFlowManager workFlowManager, string currentId, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToRoleId, string auditIdea, string workFlowId, string activityId)
        {
            int num = this.StepRoleStatrUpdata(workFlowManager, currentId, categoryCode, categoryFullName, objectId, objectFullName, sendToRoleId, auditIdea, workFlowId, activityId);
            if (num == 0)
            {
                base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            }
            return num;
        }

        private int UpdataUserStatr(IWorkFlowManager workFlowManager, string id, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToUserId, string auditIdea, string workFlowId, string activityId)
        {
            BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity = this.GetEntity(id);
            baseWorkFlowCurrentEntity.CategoryCode = categoryCode;
            baseWorkFlowCurrentEntity.CategoryFullName = categoryFullName;
            baseWorkFlowCurrentEntity.ObjectId = objectId;
            baseWorkFlowCurrentEntity.ObjectFullName = objectFullName;
            baseWorkFlowCurrentEntity.WorkFlowId = new int?(int.Parse(workFlowId));
            baseWorkFlowCurrentEntity.ActivityId = new int?(int.Parse(activityId));
            baseWorkFlowCurrentEntity.SendDate = new DateTime?(DateTime.Now);
            baseWorkFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            baseWorkFlowCurrentEntity.AuditStatus = AuditStatus.StartAudit.ToString();
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(sendToUserId);
            baseWorkFlowCurrentEntity.ToUserId = new int?(int.Parse(sendToUserId));
            baseWorkFlowCurrentEntity.ToUserRealName = entity.RealName;
            if (entity.DepartmentId > 0)
            {
                BaseOrganizeEntity entity3 = new BaseOrganizeManager(base.UserInfo).GetEntity(entity.DepartmentId);
                baseWorkFlowCurrentEntity.ToDepartmentId = entity.DepartmentId;
                baseWorkFlowCurrentEntity.ToDepartmentName = entity3.FullName;
            }
            baseWorkFlowCurrentEntity.AuditUserId = new int?(int.Parse(base.UserInfo.Id));
            baseWorkFlowCurrentEntity.AuditUserRealName = base.UserInfo.RealName;
            baseWorkFlowCurrentEntity.AuditIdea = auditIdea;
            baseWorkFlowCurrentEntity.AuditDate = new DateTime?(DateTime.Now);
            return this.UpdateEntity(baseWorkFlowCurrentEntity);
        }

        public int Update(BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity)
        {
            return this.UpdateEntity(baseWorkFlowCurrentEntity);
        }

        public int UpdateEntity(BaseWorkFlowCurrentEntity baseWorkFlowCurrentEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseWorkFlowCurrentEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowCurrentEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowCurrentEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseWorkFlowCurrentEntity.FieldId, baseWorkFlowCurrentEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        //public int UserAuditPass(IWorkFlowManager workFlowManager, string currentId, string sendToUserId, string auditIdea, int? sortCode = new int?()) //C# 4.0 才支持缺省参数
        public int UserAuditPass(IWorkFlowManager workFlowManager, string currentId, string sendToUserId, string auditIdea, int? sortCode)
        {
            if (sortCode == null) sortCode = new int?();
            int num = 0;
            num = this.StepUserAuditPass(currentId, sendToUserId, auditIdea, sortCode);
            if (num == 0)
            {
                base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            }
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = this.GetEntity(currentId);
            if (workFlowManager != null)
            {
                workFlowManager.OnAutoAuditPass(currentId, workFlowCurrentEntity.CategoryCode, auditIdea);
                workFlowManager.SendRemindMessage(workFlowCurrentEntity, AuditStatus.AuditPass, auditIdea, new string[] { sendToUserId }, null);
            }
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return num;
        }

        public string UserStatr(IWorkFlowManager workFlowManager, string categoryCode, string categoryFullName, string objectId, string objectFullName, string sendToUserId, string auditIdea, string workFlowId, string activityId)
        {
            string currentId = this.GetCurrentId(categoryCode, objectId);
            BaseWorkFlowCurrentEntity workFlowCurrentEntity = null;
            if (currentId.Length > 0)
            {
                this.UpdataUserStatr(workFlowManager, currentId, categoryCode, categoryFullName, objectId, objectFullName, sendToUserId, auditIdea, workFlowId, activityId);
                return currentId;
            }
            currentId = this.StepUserStatr(workFlowManager, categoryCode, categoryFullName, objectId, objectFullName, sendToUserId, auditIdea, workFlowId, activityId);
            workFlowCurrentEntity = this.GetEntity(currentId);
            workFlowManager.SendRemindMessage(workFlowCurrentEntity, AuditStatus.StartAudit, auditIdea, new string[] { sendToUserId }, null);
            base.ReturnStatusCode = StatusCode.OK.ToString();
            base.ReturnStatusMessage = base.GetStateMessage(base.ReturnStatusCode);
            return currentId;
        }
    }
}

