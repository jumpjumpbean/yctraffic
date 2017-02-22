using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DotNet.Business;
using WafTraffic.Domain.Common;

namespace WafTraffic.Domain
{
    public partial class ZhzxTrafficViolation
    {
        private string workflowStatusPhrase;
        public string WorkflowStatusPhrase
        {
            get
            {
                if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_PENDING)
                {
                    workflowStatusPhrase = YcConstantTable.STR_ZHZX_WORKFLOW_UPLOAD_PENDING;
                }
                else if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING)
                {
                    workflowStatusPhrase = YcConstantTable.STR_ZHZX_WORKFLOW_APPROVE_PENDING;
                }
                else if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_REJECT)
                {
                    workflowStatusPhrase = YcConstantTable.STR_ZHZX_WORKFLOW_UPLOAD_REJECT;
                }
                else if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_FILTERED)
                {
                    workflowStatusPhrase = YcConstantTable.STR_ZHZX_WORKFLOW_FILTERED;
                }
                else if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_REJECT)
                {
                    workflowStatusPhrase = YcConstantTable.STR_ZHZX_WORKFLOW_APPROVE_REJECT;
                }
                else if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED)
                {
                    workflowStatusPhrase = YcConstantTable.STR_ZHZX_WORKFLOW_APPROVED;
                }

                return workflowStatusPhrase;
            }
        }

        private string violationCode;
        public string ViolationCode
        {
            get
            {
                if (this.ViolationType == "不按车道行驶")
                {
                    violationCode = "12080";
                }
                else if (this.ViolationType == "压线")
                {
                    violationCode = "13450";
                }
                else if (this.ViolationType == "压黄线")
                {
                    violationCode = "13440";
                }
                else if (this.ViolationType == "闯红灯")
                { 
                    violationCode = "16250";
                }
                else if (this.ViolationType == "逆行")
                {
                    violationCode = "13010";
                }
                else if (this.ViolationType == "违章变道")
                {
                    violationCode = "12080";
                }
                else if (this.ViolationType == "不系安全带")
                { 
                    violationCode = "60110";
                }
                else
                {
                    violationCode = "";
                }

                return violationCode;
            }
        
        }




        private Visibility canApprove;
        public Visibility CanApprove
        {
            get
            {
                canApprove = System.Windows.Visibility.Hidden;

                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.Upload") &&
                     !CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.Check")
                ) // 只是上传人员
                {
                    if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_PENDING)  // 待上传状态时可见，其他状态时不可见
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        canApprove = System.Windows.Visibility.Hidden;
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.Check") &&
                        !CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.Upload")
                ) // 只是审核人员
                {
                    if (this.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING)  // 待审核状态时可见，其他状态时不可见
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        canApprove = System.Windows.Visibility.Hidden;
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.Check") &&
                        CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.Upload")
                )          // 既有上传权限又有审核权限
                {
                    canApprove = System.Windows.Visibility.Visible;
                }
                else
                {
                    canApprove = System.Windows.Visibility.Hidden;
                }

                return canApprove;
            }
        }
    }
}
