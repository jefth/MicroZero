// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;
using Agebull.Common.Context;
using Agebull.EntityModel.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TBusinessLogic>
        : ApiControllerForDataState<TData, TBusinessLogic>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TBusinessLogic : class, IBusinessLogicByAudit<TData>, new()
    {
        #region API

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>

        [Route("audit/deny")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult AuditDeny(IdsArguent arg)
        {

            OnAuditDeny();
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }

        /// <summary>
        ///     �������ύ�����
        /// </summary>

        [Route("audit/pullback")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult Pullback(IdsArguent arg)
        {

            OnPullback();
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }

        /// <summary>
        ///     �ύ���
        /// </summary>

        [Route("audit/submit")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult SubmitAudit(IdsArguent arg)
        {

            OnSubmitAudit();
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }

        /// <summary>
        ///     У���������
        /// </summary>

        [Route("audit/validate")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult Validate(IdsArguent arg)
        {
            if (!TryGet("selects", out long[] ids))
            {
                return ApiResult.ArgumentError;
            }

            DoValidate(ids);
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }


        /// <summary>
        ///     ���ͨ��
        /// </summary>

        [Route("audit/pass")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult AuditPass(IdsArguent arg)
        {

            OnAuditPass();
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }

        /// <summary>
        ///     �������
        /// </summary>

        [Route("audit/redo")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult UnAudit(IdsArguent arg)
        {
            OnUnAudit();
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }

        /// <summary>
        ///     �˻�(���)
        /// </summary>

        [Route("audit/back")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public ApiResult BackAudit(IdsArguent arg)
        {

            OnBackAudit();
            return IsFailed
                ? (new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : ApiResult.Ok;
        }

        #endregion

        #region ����

        /// <summary>
        ///     �ύ���
        /// </summary>
        protected virtual void OnSubmitAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!DoValidate(ids))
                return;
            if (!Business.Submit(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;

        }

        /// <summary>
        ///     �˻����
        /// </summary>
        private void OnBackAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.Back(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnUnAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.UnAudit(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected virtual void OnAuditPass()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!DoValidate(ids))
            {
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
                return;
            }
            var result = Business.AuditPass(ids);
            if (!result)
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        private bool DoValidate(IEnumerable<long> ids)
        {
            var message = new ValidateResultDictionary();
            var succeed = Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                GlobalContext.Current.LastMessage = message.ToString();
            }
            return succeed;
        }

        /// <summary>
        ///     ����
        /// </summary>
        private void OnPullback()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.Pullback(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnAuditDeny()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.AuditDeny(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }
        #endregion

        #region �б�����

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        protected override Task<ApiPageData<TData>> GetListData(LambdaItem<TData> lambda)
        {
            if (!TryGet("_audit_", out int audit) || audit == 0x100 || audit < 0)
                return base.GetListData(lambda);

            if (audit <= (int)AuditStateType.End)
            {
                lambda.AddRoot(p => p.AuditState == (AuditStateType)audit);
                return base.GetListData(lambda);
            }

            switch (audit)
            {
                case 0x10: //����
                case 0xFF: //ɾ��
                    SetArg("dataState", audit);
                    break;
                case 0x13: //ͣ��
                    SetArg("dataState", (int)DataStateType.Disable);
                    break;
                case 0x11: //δ���
                    lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    break;
                case 0x12: //δ����
                    lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                    break;
            }
            return base.GetListData(lambda);
        }
        #endregion
    }

}