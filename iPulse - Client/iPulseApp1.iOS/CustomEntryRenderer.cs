using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

namespace iPulseApp1.iOS
{
    public class CustomEntryRenderer : EntryRenderer
    {
         protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
         {

                base.OnElementChanged(e);
            if (this.Control != null)
            {
                this.Control.LeftView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                this.Control.RightView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                this.Control.LeftViewMode = UITextFieldViewMode.Always;
                this.Control.RightViewMode = UITextFieldViewMode.Always;

                this.Control.BorderStyle = UITextBorderStyle.None;
                this.Element.HeightRequest = 30;
            }
        }

    }
}