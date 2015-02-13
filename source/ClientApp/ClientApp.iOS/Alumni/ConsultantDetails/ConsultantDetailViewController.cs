using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientApp.ViewModels;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;

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

        public ConsultantDetailViewController (IntPtr handle) : base (handle)
        {
            _detailViewModel = DependencyResolver.Current.Resolve<ConsultantDetailViewModel>();
        }

	    public override void LoadView()
	    {
	        base.LoadView();
	        DetailsTable.Delegate = this;
	    }

	    public async void LoadConsultant(int id)
	    {
	        var consultant = await _detailViewModel.GetConsultant(id);
	        InvokeOnMainThread(delegate
	                           {
                                   Title = consultant.FullName;
	                               TitleLabel.Text =
	                                   consultant.Contracts.OrderByDescending(c => c.EndDate).First().Title;
	                               RatingLabel.Text = GetRatingString(consultant.Rating);
	                               ContractsLabel.Text = string.Format("{0} contracts", consultant.Contracts.Count);
                                   AddSpecializationAndSkills(consultant.Specializations, SpecializationCell);
                                   DetailsTable.ReloadData();
	                           });
            
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
	        base.PrepareForSegue(segue, sender);

	        if (segue.Identifier == "ResumeSelected")
	        {
                var view = (ResumeViewController)segue.DestinationViewController;
	            view.Resume = _detailViewModel.GetConsultant().ResumeText;
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
                    PerformSegue("ResumeSelected", this);
	                break;
                case (int)DetailsTableCells.ContractHistory:
                    //TODO Open Contract History
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
            var y = (int)specFont.LineHeight;
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
                    TextColor = new UIColor(0.1215686274509804f, 0.1215686274509804f, 0.1215686274509804f, 255),
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
            var sb = new StringBuilder();
            foreach (var skill in skills)
            {
                sb.Append(skill.Name).Append(", ");
            }
	        return sb.ToString(0, sb.Length - 2);
	    }

        private static string GetRatingString(int? rating)
	    {
            switch (rating)
	        {
                case MatchGuideConstants.ResumeRating.Standard:
                    return "Standard";
                case MatchGuideConstants.ResumeRating.AboveStandard:
	                return "Above Standard";
                case MatchGuideConstants.ResumeRating.BelowStandard:
	                return "Below Standard";
                default:
	                return "Not Checked";
	        }
        }
        #endregion
    }
}