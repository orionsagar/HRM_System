using Application.Tasks.Commands.CUserInfo;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserInfoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            // Gets the orininal Model from DataBase.
            //DatabaseModel originalRecord = _unitOfWork.DatabaseModel.Get(id);
            //var existData = await _unitOfWork.UserInfos.GetById(request.userInfoViewModel.UserID);
            //if (request.userInfoViewModel.Password == null)
            //{
            //    request.userInfoViewModel.Password = existData.Password;
            //}


            // Merge original model with the ViewModel   
            //originalRecord = _mapper.Map<ViewModel, DatabaseModel>(mappedModel, originalRecord);
            //var comp = _mapper.Map<UserInfoViewModel, UserInfo>(request.userInfoViewModel, existData);
            var comp = _mapper.Map<UserInfo>(request.userInfoViewModel);
            comp.Company = null;
            var result = await _unitOfWork.UserInfos.Update(comp);
            return result;
        }
    }
}
