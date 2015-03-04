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
	        contactButton.Clicked += (sender, args) => { PerformSegue("ContactSelected", contactButton); };
	        NavigationItem.SetRightBarButtonItem(contactButton, false);

            /* TODO
             * - Replace GreenButtonBack with the new proper logo, not sure if we need a 2x and 3x version and how that effects the next point
             * - Change the 20 inset to 3 (probably? style guide says corners should be 3 pt)
             * - replace icon-mail with the new vector version
             * */

	        var image = UIImage.FromBundle("GreenButtonBack");
	        var resizeImage = image.CreateResizableImage(new UIEdgeInsets(20, 20, 20, 20));
            OnboardButton.SetBackgroundImage(resizeImage, UIControlState.Normal);
	        OnboardButton.TouchUpInside += (sender, args) => { PerformSegue("OnboardSelected", OnboardButton); };
	    }

	    public async void LoadConsultant(int id)
	    {
            var consultant = await _detailViewModel.GetConsultant(id);
            InvokeOnMainThread(delegate{UpdateUI(consultant);});
            
	    }

	    private void UpdateUI(Consultant consultant)
	    {
	        Title = consultant.FullName;
	        TitleLabel.Text =
	            consultant.Contracts.OrderByDescending(c => c.EndDate).First().Title;

	        SetRatingImagesOrText(consultant);
	        ContractsLabel.Text = string.Format("{0} {1}", consultant.Contracts.Count,
	            consultant.Contracts.OrderByDescending(c => c.EndDate).Select(c => string.Format("({0:c}/hr)", c.Rate)).FirstOrDefault()).TrimEnd();
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
                view.Contracts = _detailViewModel.GetConsultant().Contracts;
	        } else if (segue.Identifier == "NewContractSelected")
	        {
	            var navController = (UINavigationController) segue.DestinationViewController;
	            var view = (OnboardViewController) navController.ViewControllers[0];
	            view.Consultant = _detailViewModel.GetConsultant();
            }
            else if (segue.Identifier == "ContactSelected")
            {
                var navController = (UINavigationController)segue.DestinationViewController;
                var view = (ContactAlumniViewController)navController.ViewControllers[0];
                view.Consultant = _detailViewModel.GetConsultant();
            }
	    }

	    #region Table Delegates

	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        if (indexPath.Row == (int)DetailsTableCells.SpecializationAndSkills)
	        {
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
