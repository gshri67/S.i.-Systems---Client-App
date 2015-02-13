using System;
using System.Collections.Generic;
using ClientApp.ViewModels;
using Microsoft.Practices.Unity;
using System.Linq;
using Foundation;
using Microsoft.Practices.ObjectBuilder2;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	public partial class AlumniViewController : UIViewController
	{
	    private readonly ContractorViewModel _contractorModel;

        public AlumniViewController(IntPtr handle)
            : base(handle)
        {
            _contractorModel = DependencyResolver.Current.Resolve<ContractorViewModel>();
        }

	    private void SetSummaryLabel(IEnumerable<ConsultantGroup> consultantGroup)
        {
            var alumniCount = CountAlumni(consultantGroup);
            var specializationsCount = CountSpecializations(consultantGroup);

            summaryLabel.Text = string.Format("You have {0} alumni in {1} specializations.", alumniCount, specializationsCount);
        }

        private static int CountSpecializations(IEnumerable<ConsultantGroup> consultantGroup)
        {
            return consultantGroup.Count();
        }

        private static int CountAlumni(IEnumerable<ConsultantGroup> consultantGroup)
        {
            return consultantGroup.Sum(x => x.Consultants.Count);
        }

	    #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            //set the source for our table's data
            LoadConsultantGroups();


            contractorSearch.TextChanged += delegate
            {
                //todo: set a timer/interval to fire this off after ~1 sec
                LoadConsultantGroups();
            };


            var rightButton = NavigationItem.RightBarButtonItem;

            UIImage image = new UIImage("Si-app-icon-40.png");
            image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            rightButton.SetBackgroundImage(image, UIControlState.Normal, UIBarButtonItemStyle.Plain, UIBarMetrics.Default);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

	    public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

	    private async void LoadConsultantGroups()
	    {
            //get our list of specializations to display
            var consultantGroups = await  _contractorModel.GetConsultantGroups(contractorSearch.Text);
            InvokeOnMainThread(delegate
                               {
                                   SpecializationTable.Source = new AlumniTableViewSource(this, consultantGroups);
                                   SpecializationTable.ReloadData();
                                   SetSummaryLabel(consultantGroups);
                               });
	    }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            
            if (segue.Identifier == "DisciplineSelected")
            {
                var navCtrl = segue.DestinationViewController as DisciplineViewController;

                if (navCtrl != null)
                {
                    var source = SpecializationTable.Source as AlumniTableViewSource;
                    var rowpath = SpecializationTable.IndexPathForSelectedRow;
                    var consultantGroup = source.GetItem(rowpath.Section);
                    navCtrl.SetSpecialization(this, consultantGroup);
                }
            }
        }
	}
}
