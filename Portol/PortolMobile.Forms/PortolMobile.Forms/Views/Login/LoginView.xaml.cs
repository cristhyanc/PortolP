﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortolMobile.Forms.Controls;
using PortolMobile.Forms.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PortolMobile.Forms.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginView : ExtendedContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            txtEmail.Effects.Add(new BorderEffect());
            txtPassword.Effects.Add(new BorderEffect());
        }
    }
}