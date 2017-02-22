namespace DotNet.Business
{
    using System;

    public class ServiceFactory : IServiceFactory
    {
        public IDbHelperService CreateBusinessDbHelperService()
        {
            return new BusinessDbHelperService();
        }

        public IExceptionService CreateExceptionService()
        {
            return new ExceptionService();
        }

        public IFileService CreateFileService()
        {
            return new FileService();
        }

        public IFolderService CreateFolderService()
        {
            return new FolderService();
        }

        public IItemDetailsService CreateItemDetailsService()
        {
            return new ItemDetailsService();
        }

        public IItemsService CreateItemsService()
        {
            return new ItemsService();
        }

        public ILogOnService CreateLogOnService()
        {
            return new LogOnService();
        }

        public ILogService CreateLogService()
        {
            return new LogService();
        }

        public IMessageService CreateMessageService()
        {
            return new MessageService();
        }

        public IModuleService CreateModuleService()
        {
            return new ModuleService();
        }

        public IOrganizeService CreateOrganizeService()
        {
            return new OrganizeService();
        }

        public IParameterService CreateParameterService()
        {
            return new ParameterService();
        }

        public IPermissionItemService CreatePermissionItemService()
        {
            return new PermissionItemService();
        }

        public IPermissionService CreatePermissionService()
        {
            return new PermissionService();
        }

        public IRoleService CreateRoleService()
        {
            return new RoleService();
        }

        public ISequenceService CreateSequenceService()
        {
            return new SequenceService();
        }

        public IStaffService CreateStaffService()
        {
            return new StaffService();
        }

        public ITableColumnsService CreateTableColumnsService()
        {
            return new TableColumnsService();
        }

        public IDbHelperService CreateUserCenterDbHelperService()
        {
            return new UserCenterDbHelperService();
        }

        public IUserService CreateUserService()
        {
            return new UserService();
        }

        public IWorkFlowActivityAdminService CreateWorkFlowActivityAdminService()
        {
            return new WorkFlowActivityAdminService();
        }

        public IWorkFlowCurrentService CreateWorkFlowCurrentService()
        {
            return new WorkFlowCurrentService();
        }

        public IWorkFlowProcessAdminService CreateWorkFlowProcessAdminService()
        {
            return new WorkFlowProcessAdminService();
        }

        public void InitService()
        {
        }
    }
}

