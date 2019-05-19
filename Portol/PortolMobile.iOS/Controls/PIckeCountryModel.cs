using System;
using System.Collections.Generic;
using System.Drawing;
using UIKit;

namespace PortolMobile.iOS.Controls
{
    public class PIckeCountryModel: UIPickerViewModel
    {
        private List<string> _myItems;
        protected int selectedIndex = 0;

        public PIckeCountryModel(List<string> items)
        {
            _myItems = items;
        }

        public string SelectedItem
        {
            get { return _myItems[selectedIndex]; }
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return _myItems.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return _myItems[(int)row];
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            selectedIndex = (int)row;
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            UIImage image = UIImage.FromBundle("Login_background");

            UIGraphics.BeginImageContextWithOptions(view.Frame.Size, false, 0);

            view.Layer.RenderInContext(UIGraphics.GetCurrentContext());

            //image = UIGraphics.GetImageFromCurrentImageContext();
           // UIGraphics.EndImageContext();

            return new UIImageView(image);
        }

    }
}
