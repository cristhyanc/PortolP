﻿using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PortolMobile.Core.ViewModels.SignUp;

namespace PortolMobile.Droid.Views.Signup
{
    [MvxActivityPresentation]
    [Activity(Label = "Portol",
     Theme = "@style/AppTheme",
     LaunchMode = LaunchMode.SingleTop,
     Name = "PortolMobile.Droid.Views.Signup.SignupStepCodeView"
     )]
    public class SignupStepCodeView : MvxAppCompatActivity<SignupStepCodeViewModel>
    {
        public DrawerLayout DrawerLayout { get; set; }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);

            UserDialogs.Init(this);
            SetContentView(Resource.Layout.Signup_Step_CodeView);

            EditText editText = this.FindViewById<EditText>(Resource.Id.txtFirstNumber);
            editText.TextChanged += EditText_TextChanged;

            editText = this.FindViewById<EditText>(Resource.Id.txtSecondNumber);
            editText.TextChanged += EditText_TextChanged;

            editText = this.FindViewById<EditText>(Resource.Id.txtThirdNumber);
            editText.TextChanged += EditText_TextChanged;

            editText = this.FindViewById<EditText>(Resource.Id.txtFourNumber);
            editText.TextChanged += EditText_TextChanged;

            LinearLayout layo = this.FindViewById<LinearLayout>(Resource.Id.mainLayout);
            layo.Touch += Layo_Touch;

            Button btn = this.FindViewById<Button>(Resource.Id.btnLogin);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            HideSoftKeyboard();
        }

        private void EditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (sender is EditText editText)
            {
                if (e.Text?.ToString().Length > 0)
                {
                    EditText nextText = null;

                    if (editText.Id == Resource.Id.txtFirstNumber)
                    {
                        nextText = this.FindViewById<EditText>(Resource.Id.txtSecondNumber);
                    }

                    if (editText.Id == Resource.Id.txtSecondNumber)
                    {
                        nextText = this.FindViewById<EditText>(Resource.Id.txtThirdNumber);
                    }

                    if (editText.Id == Resource.Id.txtThirdNumber)
                    {
                        nextText = this.FindViewById<EditText>(Resource.Id.txtFourNumber);
                    }

                    if (editText.Id == Resource.Id.txtFourNumber)
                    {
                        HideSoftKeyboard();
                    }

                    if (nextText != null)
                    {
                        nextText.RequestFocus();
                    }
                }
            }


        }

        private void Layo_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            HideSoftKeyboard();
        }

        public void HideSoftKeyboard()
        {
            try
            {
                if (CurrentFocus == null)
                    return;

                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

                CurrentFocus.ClearFocus();
            }
            catch (System.Exception)
            {

            }

        }

    }

}
