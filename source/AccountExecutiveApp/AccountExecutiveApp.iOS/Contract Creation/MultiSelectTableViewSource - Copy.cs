using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class MultiSelectTableViewSource : UITableViewSource
    {
        private readonly MultiSelectTableViewController _parentController;

        public List<UserContact> Contacts = new List<UserContact>();
        public List<UserContact> Selected = new List<UserContact>();

        public MultiSelectTableViewSource(MultiSelectTableViewController parentController)
        {
            _parentController = parentController;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("UITableViewCell") as UITableViewCell;

            cell.TextLabel.Text = Contacts[(int)indexPath.Item].FullName;

            UserContact curContact = Contacts[(int) indexPath.Item];

            if ( Selected.Any(c => c.Id == curContact.Id))
                tableView.SelectRow(indexPath, true, UITableViewScrollPosition.None);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if( Contacts != null )
                return Contacts.Count;
            return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}