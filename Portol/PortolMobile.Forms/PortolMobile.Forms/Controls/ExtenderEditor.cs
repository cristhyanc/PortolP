using PortolMobile.Forms.Effects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PortolMobile.Forms.Controls
{
   public class ExtenderEditor:Editor
    {
        public static readonly BindableProperty OnFocusCommandProperty =
              BindableProperty.Create(nameof(OnFocusCommand), typeof(ICommand), typeof(ExtendedEntry), null);

        public ICommand OnFocusCommand
        {
            get { return (ICommand)GetValue(OnFocusCommandProperty); }
            set { SetValue(OnFocusCommandProperty, value); }
        }

        public static readonly BindableProperty OnUnfocusCommandProperty =
       BindableProperty.Create(nameof(OnUnfocusCommand), typeof(ICommand), typeof(ExtendedEntry), null);

        public ICommand OnUnfocusCommand
        {
            get { return (ICommand)GetValue(OnUnfocusCommandProperty); }
            set { SetValue(OnUnfocusCommandProperty, value); }
        }

        public static readonly BindableProperty TextChangedCommandProperty =
        BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(ExtendedEntry), null);

        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly BindableProperty OnFocusCommandParameterProperty =
        BindableProperty.Create(nameof(OnFocusCommandParameter), typeof(string), typeof(ExtendedEntry), null);

        public string OnFocusCommandParameter
        {
            get { return (string)GetValue(OnFocusCommandParameterProperty); }
            set { SetValue(OnFocusCommandParameterProperty, value); }
        }

        public ExtenderEditor()
        {
            this.Effects.Add(new BorderlessEffect());
            this.Focused += ExtendedEntry_Focused;
            this.TextChanged += ExtendedEntry_TextChanged;
            this.Unfocused += ExtendedEntry_Unfocused;
        }

        private void ExtendedEntry_Unfocused(object sender, FocusEventArgs e)
        {
            Execute(OnUnfocusCommand, null);
        }

        private void ExtendedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Execute(TextChangedCommand, this.Text);
        }

        private void ExtendedEntry_Focused(object sender, FocusEventArgs e)
        {
            Execute(OnFocusCommand, OnFocusCommandParameter);
        }
        public void Execute(ICommand command, string parameter)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {

                command.Execute(parameter);
            }
        }

    }
}
