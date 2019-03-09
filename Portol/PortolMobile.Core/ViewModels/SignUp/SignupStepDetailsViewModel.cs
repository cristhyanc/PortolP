using MvvmCross.Commands;
using MvvmCross.Navigation;
using Portol.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Core.ViewModels.SignUp
{
    public class SignupStepDetailsViewModel : BaseViewModel<UserDto>
    {
        public IMvxCommand GotoEmailPageCommand { get; private set; }
        private readonly IMvxNavigationService _navigationService;
        UserDto _userDto;

        public SignupStepDetailsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            GotoEmailPageCommand = new MvxAsyncCommand(GotoEmailPage);
        }

        private async Task GotoEmailPage()
        {

        }

        public override void Prepare(UserDto parameter)
        {
            _userDto = parameter;
        }
    }
}
