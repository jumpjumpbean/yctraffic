namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class UserBillManager : BaseNewsManager, IWorkFlowManager
    {
        public UserBillManager()
        {
            base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            base.CurrentTableName = "WorkFlow_UserBill";
        }

        public UserBillManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public UserBillManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public UserBillManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public UserBillManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public UserBillManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public int AfterAutoStatr(string id)
        {
            return 0;
        }

        public int AuditQuash(string id, string auditIdea)
        {
            int num = 0;
            BaseNewsEntity entity = base.GetEntity(id);
            if (((entity.AuditStatus != null) && !entity.AuditStatus.Equals(AuditStatus.StartAudit.ToString())) && !entity.AuditStatus.Equals(AuditStatus.WaitForAudit.ToString()))
            {
                return num;
            }
            string categoryCode = entity.CategoryCode;
            string title = entity.Title;
            string text2 = entity.Title;
            string text3 = base.UserInfo.Id + "_" + entity.FolderId;
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.DbHelper, base.UserInfo);
            return manager.AuditQuash(this, categoryCode, id, auditIdea);
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
            string str = string.Empty;
            WorkFlowInfo info = this.BeforeAutoStatr(id, false);
            if (info != null)
            {
                string categoryCode = info.CategoryCode;
                string categoryFullName = info.CategoryFullName;
                string objectFullName = info.ObjectFullName;
                string workFlowCode = info.WorkFlowCode;
                str = new BaseWorkFlowCurrentManager(base.DbHelper, base.UserInfo).AutoStatr(this, categoryCode, categoryFullName, id, objectFullName, auditIdea, dtWorkFlowActivity);
                if (!string.IsNullOrEmpty(str))
                {
                    this.AfterAutoStatr(id);
                }
            }
            return str;
        }


        //public string AutoStatr(string id, string auditIdea, string toUserId = null) //C# 4.0 才支持缺省参数
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

        public WorkFlowInfo BeforeAutoStatr(string id)
        {
            return this.BeforeAutoStatr(id, true);
        }

        //public WorkFlowInfo BeforeAutoStatr(string id, bool checkedActivity = true) //C# 4.0 才支持缺省参数
        public WorkFlowInfo BeforeAutoStatr(string id, bool checkedActivity) 
        {
            WorkFlowInfo info = null;
            BaseNewsEntity baseNewsEntity = base.GetEntity(id);
            if ((!string.IsNullOrEmpty(baseNewsEntity.AuditStatus) && !baseNewsEntity.AuditStatus.Equals(AuditStatus.Draft.ToString())) && !baseNewsEntity.AuditStatus.Equals(AuditStatus.AuditReject.ToString()))
            {
                return null;
            }
            info = new WorkFlowInfo {
                CategoryCode = baseNewsEntity.CategoryCode,
                ObjectId = baseNewsEntity.Id
            };
            if (string.IsNullOrEmpty(baseNewsEntity.Code))
            {
                info.ObjectFullName = baseNewsEntity.Title;
            }
            else
            {
                info.ObjectFullName = baseNewsEntity.Code + " " + baseNewsEntity.Title;
            }
            info.CategoryFullName = new TemplateManager(base.DbHelper, base.UserInfo).GetProperty(baseNewsEntity.CategoryCode, BaseNewsEntity.FieldTitle);
            info.WorkFlowCode = base.UserInfo.Id + "_" + baseNewsEntity.CategoryCode;
            if (checkedActivity)
            {
                string str = new BaseWorkFlowProcessManager(base.DbHelper, base.UserInfo).GetId(BaseWorkFlowProcessEntity.FieldCode, info.WorkFlowCode, BaseWorkFlowProcessEntity.FieldDeletionStateCode, 0);
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                BaseWorkFlowActivityManager manager3 = new BaseWorkFlowActivityManager(base.DbHelper, base.UserInfo);
                string[] names = new string[] { BaseWorkFlowActivityEntity.FieldWorkFlowId, BaseWorkFlowActivityEntity.FieldDeletionStateCode, BaseWorkFlowActivityEntity.FieldAuditUserId };
                object[] values = new object[] { str, 0, "Anyone" };
                if (manager3.Exists(names, values))
                {
                    return null;
                }
            }
            string fullName = "UserTemplateBill";
            string newValue = string.Empty;
            BaseSequenceManager manager4 = new BaseSequenceManager(base.DbHelper, base.UserInfo);
            if ((!string.IsNullOrEmpty(baseNewsEntity.Source) && baseNewsEntity.Source.Equals("html")) && (baseNewsEntity.Contents.IndexOf("#DayCode#") > 0))
            {
                fullName = DateTime.Now.ToString("yyyyMMdd") + "_" + baseNewsEntity.CategoryCode;
                newValue = DateTime.Now.ToString("yyyyMMdd") + "_" + manager4.GetSequence(fullName, 1, 3, true);
                baseNewsEntity.Contents = baseNewsEntity.Contents.Replace("#DayCode#", newValue);
                baseNewsEntity.Code = newValue;
            }
            if (string.IsNullOrEmpty(newValue))
            {
                newValue = manager4.GetSequence(fullName, 1, 6, true);
                baseNewsEntity.Code = newValue;
            }
            baseNewsEntity.AuditStatus = AuditStatus.StartAudit.ToString();
            baseNewsEntity.Enabled = 0;
            base.Update(baseNewsEntity);
            return info;
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
                BaseNewsEntity baseNewsEntity = base.GetEntity(objectId);
                baseNewsEntity.Enabled = 1;
                baseNewsEntity.AuditStatus = AuditStatus.AuditComplete.ToString();
                base.Update(baseNewsEntity);
            }
            return true;
        }

        public bool OnAuditQuash(string currentId, string categoryCode, string auditIdea)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string objectId = manager.GetEntity(currentId).ObjectId;
            BaseNewsEntity baseNewsEntity = base.GetEntity(objectId);
            baseNewsEntity.Enabled = 0;
            baseNewsEntity.DeletionStateCode = 1;
            baseNewsEntity.AuditStatus = AuditStatus.AuditQuash.ToString();
            base.Update(baseNewsEntity);
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

        public bool OnAuditReject(string[] currentIds, string categoryCode, string auditIdea)
        {
            for (int i = 0; i < currentIds.Length; i++)
            {
                this.OnAuditReject(currentIds[i], categoryCode, auditIdea);
            }
            return true;
        }

        public bool OnAuditReject(string currentId, string categoryCode, string auditIdea)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string objectId = manager.GetEntity(currentId).ObjectId;
            if (!string.IsNullOrEmpty(objectId))
            {
                BaseNewsEntity baseNewsEntity = base.GetEntity(objectId);
                baseNewsEntity.Enabled = 0;
                baseNewsEntity.AuditStatus = AuditStatus.AuditReject.ToString();
                base.UpdateEntity(baseNewsEntity);
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
                BaseNewsEntity baseNewsEntity = base.GetEntity(objectId);
                baseNewsEntity.AuditStatus = AuditStatus.AuditPass.ToString();
                base.UpdateEntity(baseNewsEntity);
            }
            return true;
        }

        public DataTable SearchBill(string userId, string categoryId, string categorybillFullName, string searchValue, bool? enabled, bool? deletionStateCode)
        {
            int num = 0;
            if (deletionStateCode.HasValue)
            {
                num = deletionStateCode.Value ? 1 : 0;
            }
            string commandText = string.Empty;
            commandText = string.Concat(new object[] { 
                " SELECT ", BaseNewsEntity.FieldId, "        ,", BaseNewsEntity.FieldCategoryCode, "        ,", BaseNewsEntity.FieldTitle, "        ,", BaseNewsEntity.FieldCode, "        ,", BaseNewsEntity.FieldIntroduction, "        ,", BaseNewsEntity.FieldAuditStatus, "        ,", BaseNewsEntity.FieldModifiedUserId, "        ,", BaseNewsEntity.FieldModifiedBy, 
                "        ,", BaseNewsEntity.FieldModifiedOn, "        ,", BaseNewsEntity.FieldSortCode, " FROM ", base.CurrentTableName, " WHERE ", BaseNewsEntity.FieldDeletionStateCode, " = ", num
             });
            if (enabled.HasValue)
            {
                int num2 = enabled.Value ? 1 : 0;
                object obj2 = commandText;
                commandText = string.Concat(new object[] { obj2, " AND ", BaseNewsEntity.FieldEnabled, " = ", num2 });
            }
            if (!string.IsNullOrEmpty(userId))
            {
                string str3 = commandText;
                commandText = str3 + " AND " + BaseNewsEntity.FieldCreateUserId + " = " + userId;
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                TemplateManager manager = new TemplateManager(base.DbHelper, base.UserInfo);
                string str2 = BaseBusinessLogic.FieldToList(manager.Search(string.Empty, categoryId, string.Empty, null, false), BaseFileEntity.FieldId);
                if (!string.IsNullOrEmpty(str2))
                {
                    string str4 = commandText;
                    commandText = str4 + " AND (" + BaseNewsEntity.FieldCategoryCode + " IN (" + str2 + ")) ";
                }
            }
            if (!string.IsNullOrEmpty(categorybillFullName))
            {
                string str5 = commandText;
                commandText = str5 + " AND " + BaseNewsEntity.FieldCategoryCode + " = '" + categorybillFullName + "'";
            }
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            searchValue = searchValue.Trim();
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str6 = commandText;
                string str7 = str6 + " AND (" + BaseNewsEntity.FieldTitle + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldTitle);
                string str8 = str7 + " OR " + BaseNewsEntity.FieldCode + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldCode);
                string str9 = str8 + " OR " + BaseNewsEntity.FieldCategoryCode + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldCategoryCode);
                string str10 = str9 + " OR " + BaseNewsEntity.FieldModifiedBy + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldModifiedBy);
                commandText = str10 + " OR " + BaseNewsEntity.FieldIntroduction + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldIntroduction) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldTitle, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldCode, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldCategoryCode, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldModifiedBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldIntroduction, searchValue));
            }
            commandText = commandText + " ORDER BY " + BaseNewsEntity.FieldSortCode + " DESC ";
            return base.DbHelper.Fill(commandText, list.ToArray());
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

