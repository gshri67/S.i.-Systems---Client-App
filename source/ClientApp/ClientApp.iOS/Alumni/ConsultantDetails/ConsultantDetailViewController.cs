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
using System.Threading.Tasks;

namespace ClientApp.iOS
{
    partial class ConsultantDetailViewController : UITableViewController, IUITableViewDelegate
    {
        private const int Margin = 20;
        private enum DetailsTableCells
		{
		    TitleAndContact = 0,
            SpecializationAndSkills = 1,
            ReferenceRating = 2,
            Resume = 3,
            ContractHistory = 4
		}

        private Task _initializing;
        private Func<Task> loadFn;
        private readonly ConsultantDetailViewModel _detailViewModel;
	    private LoadingOverlay _overlay;
        private string _emailText;
        private const string RateWitheldText = "Rate Withheld";

        public ConsultantDetailViewController (IntPtr handle) : base (handle)
        {
            _detailViewModel = DependencyResolver.Current.Resolve<ConsultantDetailViewModel>();
        }

	    public override void ViewDidLoad()
	    {
            base.ViewDidLoad();
	        DetailsTable.Delegate = this;
            _overlay = new LoadingOverlay(DetailsTable.Frame, retryFn: async () =>
            {
                await loadFn();
                InvokeOnMainThread(UpdateUI);
            });
            View.Add(_overlay);

            ContactButton.Layer.CornerRadius = StyleGuideConstants.ButtonCornerRadius;
            ContactButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            ContactButton.SetTitleColor(StyleGuideConstants.LightGrayUiColor, UIControlState.Highlighted);

            OnboardButton.Layer.CornerRadius = StyleGuideConstants.ButtonCornerRadius;
            OnboardButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            OnboardButton.SetTitleColor(StyleGuideConstants.LightGrayUiColor, UIControlState.Highlighted);
            OnboardButton.TouchUpInside += (sender, args) =>
	        {
	            PerformSegue("OnboardSelected", OnboardButton);
	        };

            Title = _detailViewModel.Title;

            _initializing.ContinueWith(_ => InvokeOnMainThread(UpdateUI), TaskContinuationOptions.OnlyOnRanToCompletion);
            _initializing.ContinueWith(_ => InvokeOnMainThread(() => _overlay.SetFailedState()), TaskContinuationOptions.OnlyOnFaulted);
	    }

        public void Initialize(ConsultantSummary summary)
        {
            loadFn = () => this._detailViewModel.Initialize(summary);
            _initializing = loadFn();
        }

        private void UpdateUI()
        {
            OnboardButton.SetTitle(_detailViewModel.ActionText, UIControlState.Normal);
            ContactButton.TouchUpInside += (sender, args) =>
            {
                if (_detailViewModel.IsActive)
                {
                    _emailText = string.Empty;
                    PerformSegue("ContactSelected", ContactButton);
                }
                else
                {
                    PopUpCannedMessageView();
                }
            };

            if (_detailViewModel.RatingAsInt == 0)
            {
                RatingLabel.Text = _detailViewModel.RatingAsString;
            }
            else
            {
                var starImageViews = new[] { LeftStar, MiddleStar, RightStar };

                for (int i = 1; i <= starImageViews.Length; i++)
                {
                    var imageView = starImageViews[i - 1];
                    imageView.Image = i <= _detailViewModel.RatingAsInt
                        ? UIImage.FromBundle("goldstar")
                        : UIImage.FromBundle("star");
               }
            }
            TitleLabel.Text = _detailViewModel.TitleLabel;
            TitleLabel.SizeToFit();
            ResumeLabel.Text = _detailViewModel.ResumeLabel;
            ResumeCell.Accessory = _detailViewModel.HasResume
                ? UITableViewCellAccessory.DisclosureIndicator
                : UITableViewCellAccessory.None;

            AddSpecializationAndSkills(_detailViewModel.Specializations, SpecializationCell);

            DetailsTable.ReloadData();

            _overlay.Hide();
        }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
	        base.PrepareForSegue(segue, sender);

	        if (segue.Identifier == "ResumeSelected")
	        {
                var view = (ResumeViewController)segue.DestinationViewController;
	            view.Resume = _detailViewModel.Consultant.ResumeText;
	        } else if (segue.Identifier == "ContractsSelected")
	        {
                var view = (ContractsViewController)segue.DestinationViewController;
                view.Contracts = _detailViewModel.Consultant.Contracts.ToList();
	        } else if (segue.Identifier == "OnboardSelected")
	        {
	            var navController = (UINavigationController) segue.DestinationViewController;
	            var view = (OnboardViewController) navController.ViewControllers[0];
                view.Consultant = _detailViewModel.Consultant;
	            view.IsActiveConsultant = _detailViewModel.IsActive;
	        }
            else if (segue.Identifier == "ContactSelected")
            {
                var navController = (UINavigationController)segue.DestinationViewController;
                var view = (ContactAlumniViewController)navController.ViewControllers[0];
                view.Consultant = _detailViewModel.Consultant;
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
            var skillFont = UIFont.SystemFontOfSize(15f);
            var skillIconFont = UIFont.SystemFontOfSize(8f);
            var frame = cell.Frame;
            var specializations = specs as IList<Specialization> ?? specs.ToList();
            var y = specializations.Any() ? (int)specFont.LineHeight : 0;
            foreach (var spec in specializations)
	        {
	            var specLabel = new UILabel {Text = spec.Name, Frame = new CGRect(20, y, frame.Width - 40, specFont.LineHeight), Font = specFont};
	            cell.Add(specLabel);
                y += (int)specFont.LineHeight + 2;
	            var sortedSkills = spec.Skills.OrderByDescending(s => (int) s.YearsOfExperience).ThenBy(s => s.Name);
	            foreach (var skill in sortedSkills)
	            {
                    var skillIcon = new UIImageView(GetImageForSkill(skill))
                    {
                        Frame = new CGRect(Margin, y + 4, 40, 3)
                    };
	                var skillIconText = new UILabel
	                {
                        Text = skill.YearsOfExperience.ToString(),
                        Font = skillIconFont,
                        Frame = new CGRect(Margin, skillIcon.Frame.GetMaxY(), 40, skillIconFont.LineHeight)
	                };
                    var skillLabel = new UILabel
                    {
                        Text = skill.Name,
                        Frame = new CGRect(skillIcon.Frame.GetMaxX() + 5, y, frame.Width - skillIcon.Frame.GetMaxX() - 5 - Margin, skillFont.LineHeight),
                        Font = skillFont,
                        TextColor = StyleGuideConstants.DarkGrayUiColor
                    };
                    skillLabel.SizeToFit();
	                y += (int)skillLabel.Frame.Height + 2;
                    cell.Add(skillIcon);
                    cell.Add(skillIconText);
                    cell.Add(skillLabel);
	            }
                y += (int)specFont.LineHeight;

	        }
            frame.Height = y;
            cell.Frame = frame;
        }

        private static UIImage GetImageForSkill(Skill skill)
        {
            switch (skill.YearsOfExperience)
            {
                case MatchGuideConstants.YearsOfExperience.LessThanTwo:
                    return UIImage.FromBundle("lt2.png");
                case MatchGuideConstants.YearsOfExperience.TwoToFour:
                    return UIImage.FromBundle("2to4.png");
                case MatchGuideConstants.YearsOfExperience.FiveToSeven:
                    return UIImage.FromBundle("5to7.png");
                case MatchGuideConstants.YearsOfExperience.EightToTen:
                    return UIImage.FromBundle("8to10.png");
                case MatchGuideConstants.YearsOfExperience.MoreThanTen:
                    return UIImage.FromBundle("gt10.png");
                default:
                    return UIImage.FromBundle("lt2.png");                    
            }
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
