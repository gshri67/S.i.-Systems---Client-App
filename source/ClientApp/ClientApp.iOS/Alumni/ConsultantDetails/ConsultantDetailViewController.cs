using System;
using System.Collections.Generic;
using System.Linq;
using ClientApp.iOS.Alumni;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;
using ClientApp.Core.ViewModels;

namespace ClientApp.iOS
{
    partial class ConsultantDetailViewController : UITableViewController, IUITableViewDelegate
	{
		private enum DetailsTableCells
		{
		    TitleAndContact = 0,
            SpecializationAndSkills = 1,
            ReferenceRating = 2,
            Resume = 3,
            ContractHistory = 4
		}

        private readonly ConsultantDetailViewModel _detailViewModel;
	    private LoadingOverlay _overlay;
        private string _emailText;
        private const string RateWitheldText = "Rate Withheld";

        public ConsultantDetailViewController (IntPtr handle) : base (handle)
        {
            _detailViewModel = DependencyResolver.Current.Resolve<ConsultantDetailViewModel>();
        }

	    public override void LoadView()
	    {
	        base.LoadView();
	        DetailsTable.Delegate = this;
	        if (_detailViewModel.IsLoading)
	        {
	            _overlay = new LoadingOverlay(DetailsTable.Frame);
                View.Add(_overlay);
	        }

	        var contactButton = new UIBarButtonItem
	                            {
	                                Image = UIImage.FromBundle("icon-mail"),
	                                TintColor = StyleGuideConstants.RedUiColor
	                            };
            contactButton.Clicked += (sender, args) =>
	                                 {
	                                     if (_detailViewModel.IsActiveConsultant)
	                                     {
	                                         _emailText = string.Empty;
	                                         PerformSegue("ContactSelected", contactButton);
	                                     }
	                                     else
	                                     {
	                                         PopUpCannedMessageView();
	                                     }
	                                 };
	        NavigationItem.SetRightBarButtonItem(contactButton, false);

            OnboardButton.Layer.CornerRadius = StyleGuideConstants.ButtonCornerRadius;
            OnboardButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            OnboardButton.TouchUpInside += (sender, args) =>
	        {
	            PerformSegue("OnboardSelected", OnboardButton);
	        };
	    }

	    public async void LoadConsultant(int id, bool isActiveConsultant)
	    {
	        _detailViewModel.IsActiveConsultant = isActiveConsultant;
            var consultant = await _detailViewModel.GetConsultant(id);

            InvokeOnMainThread(() =>
            {
                if (consultant == null)
                {
                    this.NavigationController.PopViewController(true);
                }
                else
                {
                    UpdateUI(consultant);
                }
            });
	    }

	    private void UpdateUI(Consultant consultant)
	    {
	        Title = consultant.FullName;
	        var lastContract =
	            consultant.Contracts.Where(c => !string.IsNullOrEmpty(c.Title)).OrderByDescending(c => c.EndDate).FirstOrDefault();
            TitleLabel.Text = lastContract != null ? lastContract.Title : "";
            TitleLabel.SizeToFit();
	        if (_detailViewModel.IsActiveConsultant)
	        {
	            OnboardButton.SetTitle("Renew", UIControlState.Normal);
	        }

	        SetRatingImagesOrText(consultant);
	        ContractsLabel.Text = string.Format("{0} ({1})", consultant.Contracts.Count(),
	            consultant.Contracts.OrderByDescending(c => c.EndDate).Select(c => c.RateWitheld ? RateWitheldText : string.Format("{0:c}/hr", c.Rate)).FirstOrDefault()).TrimEnd();
	        AddSpecializationAndSkills(consultant.Specializations, SpecializationCell);

	        if (string.IsNullOrEmpty(consultant.ResumeText))
	        {
	            ResumeLabel.Text = "No Resume";
                ResumeCell.Accessory = UITableViewCellAccessory.None;
	        }
	        else
	        {
	            ResumeLabel.Text = "Resume";
                ResumeCell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
	        }
	        
            DetailsTable.ReloadData();
            
            if (_overlay != null)
	        {
	            _overlay.Hide();
	        }
	    }

	    private void SetRatingImagesOrText(Consultant consultant)
	    {
	        if (consultant.Rating == MatchGuideConstants.ResumeRating.NotChecked ||
	            consultant.Rating == MatchGuideConstants.ResumeRating.AlsoNotChecked)
	            RatingLabel.Text = consultant.Rating.ToString();
            else { 
	            var ratingImageFetcher = new RatingImage(consultant.Rating);
                LeftStar.Image = ratingImageFetcher.GetFirstStar();
                MiddleStar.Image = ratingImageFetcher.GetSecondStar();
                RightStar.Image = ratingImageFetcher.GetThirdStar();
            }
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
	        base.PrepareForSegue(segue, sender);

	        if (segue.Identifier == "ResumeSelected")
	        {
                var view = (ResumeViewController)segue.DestinationViewController;
	            view.Resume = _detailViewModel.GetConsultant().ResumeText;
	        } else if (segue.Identifier == "ContractsSelected")
	        {
                var view = (ContractsViewController)segue.DestinationViewController;
                view.Contracts = _detailViewModel.GetConsultant().Contracts.ToList();
	        } else if (segue.Identifier == "OnboardSelected")
	        {
	            var navController = (UINavigationController) segue.DestinationViewController;
	            var view = (OnboardViewController) navController.ViewControllers[0];
	            view.Consultant = _detailViewModel.GetConsultant();
	            view.IsActiveConsultant = _detailViewModel.IsActiveConsultant;
	        }
            else if (segue.Identifier == "ContactSelected")
            {
                var navController = (UINavigationController)segue.DestinationViewController;
                var view = (ContactAlumniViewController)navController.ViewControllers[0];
                view.Consultant = _detailViewModel.GetConsultant();
                view.InitialEmailText = _emailText;
            }
	    }

        private void PopUpCannedMessageView()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var controller = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
                var newContractAction = UIAlertAction.Create("Are you available?", UIAlertActionStyle.Default,
                    action =>
                    {
                        _emailText = "Are you available for a new contract? ";
                        PerformSegue("ContactSelected", controller);
                    });
                var catchupAction = UIAlertAction.Create("Let's catch up.", UIAlertActionStyle.Default,
                    action =>
                    {
                        _emailText = "Let's catch up. ";
                        PerformSegue("ContactSelected", controller);
                    });
                var customAction = UIAlertAction.Create("Custom Message...", UIAlertActionStyle.Default,
                    action =>
                    {
                        _emailText = "";
                        PerformSegue("ContactSelected", controller);
                    });
                var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);

                controller.AddAction(newContractAction);
                controller.AddAction(catchupAction);
                controller.AddAction(customAction);
                controller.AddAction(cancelAction);
                PresentViewController(controller, true, null);
            }
            else
            {
                var sheet = new UIActionSheet();
                sheet.AddButton("Are you available?");
                sheet.AddButton("Let's catch up.");
                sheet.AddButton("Custom Message...");
                sheet.AddButton("Cancel");
                sheet.CancelButtonIndex = 3;
                sheet.Clicked += (sender, args) =>
                                 {
                                     switch (args.ButtonIndex)
                                     {
                                         case 0:
                                             _emailText = "Are you available for a new contract? ";
                                             PerformSegue("ContactSelected", sheet);
                                             break;
                                         case 1:
                                             _emailText = "Let's catch up. ";
                                             PerformSegue("ContactSelected", sheet);
                                             break;
                                         case 2:
                                             _emailText = "";
                                             PerformSegue("ContactSelected", sheet);
                                             break;
                                     }
                                 };
                sheet.ShowInView(View);
            }
            
        }

        #region Table Delegates

	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        switch (indexPath.Row)
	        {
	            case (int)DetailsTableCells.TitleAndContact:
	                return string.IsNullOrEmpty(TitleLabel.Text) ? 56 : (60 + TitleLabel.Frame.Height);
	            case (int)DetailsTableCells.SpecializationAndSkills:
	                return SpecializationCell.Frame.Height;
	        }
	        return base.GetHeightForRow(tableView, indexPath);
	    }

	    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
	    {
	        switch (indexPath.Row)
	        {
                case (int)DetailsTableCells.Resume:
	                if (ResumeLabel.Text != "No Resume")
	                {
	                    PerformSegue("ResumeSelected", this);
	                }
	                break;
                case (int)DetailsTableCells.ContractHistory:
                    PerformSegue("ContractsSelected", this);
	                break;
	        }
            tableView.DeselectRow(indexPath, true);
	    }

	    #endregion

        #region Specialization And Skills cell
        private void AddSpecializationAndSkills(IEnumerable<Specialization> specs, UITableViewCell cell)
        {
            var specFont = UIFont.SystemFontOfSize(17f);
            var skillFont = UIFont.SystemFontOfSize(14f);
            var frame = cell.Frame;
            var y = specs.Any() ? (int)specFont.LineHeight : 0;
            foreach (var spec in specs)
	        {
	            var specLabel = new UILabel {Text = spec.Name, Frame = new CGRect(20, y, frame.Width - 40, specFont.LineHeight), Font = specFont};
	            cell.Add(specLabel);
                y += (int)specFont.LineHeight;
                var skillLabel = new UILabel
                {
                    Text = GetSkillsString(spec.Skills),
                    Frame = new CGRect(20, y, frame.Width - 40, skillFont.LineHeight),
                    Font = skillFont,
                    TextColor = StyleGuideConstants.DarkGrayUiColor,
                    Lines = 0,
                    LineBreakMode = UILineBreakMode.WordWrap
                };
                skillLabel.SizeToFit();
	            y += (int)skillLabel.Frame.Height;
	            cell.Add(skillLabel);
                y += (int)specFont.LineHeight;
	        }
            frame.Height = y;
            cell.Frame = frame;
        }

	    private static string GetSkillsString(IEnumerable<Skill> skills)
	    {
	        var lines =
	            skills.OrderByDescending(s => (int) s.YearsOfExperience)
	                .ThenBy(s => s.Name)
	                .Select(skill => string.Format("{0} {1}", skill.Name, skill.YearsOfExperience));

	        return string.Join("\n", lines);
	    }
        #endregion
	}
}
