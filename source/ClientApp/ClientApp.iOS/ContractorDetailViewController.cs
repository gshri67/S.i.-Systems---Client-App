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
	partial class ContractorDetailViewController : UITableViewController, IUITableViewDelegate
	{
		private readonly ContractorDetailViewModel _detailViewModel;

        public ContractorDetailViewController (IntPtr handle) : base (handle)
        {
            _detailViewModel = DependencyResolver.Current.Resolve<ContractorDetailViewModel>();
        }

	    public override void LoadView()
	    {
	        base.LoadView();
	        DetailsTable.Delegate = this;
	    }

        //Resize the skills cell to fit it's contents
	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        if (indexPath.Row == 1)
	        {
	            return SpecializationCell.Frame.Height;
	        }
	         return base.GetHeightForRow(tableView, indexPath);
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

        private void AddSpecializationAndSkills(IEnumerable<Specialization> specs, UITableViewCell cell)
        {
            var specFont = UIFont.SystemFontOfSize(17f);
            var skillFont = UIFont.SystemFontOfSize(14f);
            var frame = cell.Frame;
            var y = 8;
            foreach (var spec in specs)
	        {
	            var specLabel = new UILabel {Text = spec.Name, Frame = new CGRect(20, y, frame.Width - 40, specFont.LineHeight), Font = specFont};
	            cell.Add(specLabel);
                y += (int)specFont.LineHeight;
                var skillLabel = new UILabel
                {
                    Text = GetSkillsString(spec.Skills),
                    Frame = new CGRect(20, y, frame.Width- 40, skillFont.LineHeight),
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
	}
}
