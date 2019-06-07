using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PortolMobile.Forms.Controls
{
    public class RatingStars : ContentView
    {
        private Label ReviewsLabel { get; set; }
        private List<Image> StarImages { get; set; }

        public RatingStars()
        {
            GenerateDisplay();
        }

        private void GenerateDisplay()
        {

            //Creates Review Count Label 
            ReviewsLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
                      
            //Create Horizontal Stack containing stars and review count label 
            StackLayout starsStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                Padding = 0,
                Spacing = 0,              
            };

            StarImages = new List<Image>();
            for (int i = 1; i <= this.TotalStars; i++)
            {
                var ima = new Image();
                TapGestureRecognizer tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += TapGesture_Tapped;
                ima.GestureRecognizers.Add(tapGesture);
                ima.Source = GetStarFileName(i);
                StarImages.Add(ima);
                starsStack.Children.Add(ima);
            }
                       
            ReviewsLabel.Text = CurrentReview > 0 ? " (" + Convert.ToString(CurrentReview) + ")" : "";         

            this.Content = starsStack;
        }

        private void SetImageStar()
        {
            for (int i = 1; i <= this.TotalStars; i++)
            {
                StarImages[i - 1].Source = GetStarFileName(i);
            }
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            CurrentReview = StarImages.IndexOf(img) + 1;
            SetImageStar();
        }


        //uses zero based position for stars 
        private string GetStarFileName(int position)
        {  
            if (CurrentReview >= position)
            {
                return "rating_star_on.png";
            }            
            else
            {
                return "rating_star_off.png";
            }
        }


        public static readonly BindableProperty TotalStarsProperty =
        BindableProperty.Create(nameof(TotalStars), typeof(int), typeof(RatingStars),0,BindingMode.OneWay , propertyChanged:(bindable, oldValue, newValue) => {
            var ratingStars = (RatingStars)bindable;
            ratingStars.GenerateDisplay();
        });
     
        //Rating is out of 10 
        public int TotalStars
        {
            get { return (int)GetValue(TotalStarsProperty); }
            set { SetValue(TotalStarsProperty, value); }
        }


        public static readonly BindableProperty CurrentReviewProperty =
        BindableProperty.Create(nameof(CurrentReview), typeof(int), typeof(RatingStars), 0, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => {
          var ratingStars = (RatingStars)bindable;
          ratingStars.GenerateDisplay();
        });
        
       
        public int CurrentReview
        {
            get { return (int)GetValue(CurrentReviewProperty); }
            set { SetValue(CurrentReviewProperty, value); }
        }

    }
}
