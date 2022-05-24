using ReadingChecklistWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingChecklistWpf.ViewModels.Cmds
{
    public class ClearSelectedTagsCommand : CommandBase
    {
        private readonly TagListViewModel _tagListViewModel;

        public ClearSelectedTagsCommand(TagListViewModel tagListViewModel)
        {
            _tagListViewModel = tagListViewModel;
        }
        public override bool CanExecute(object? parameter)
        {
            bool areThereSelectedTags = _tagListViewModel.SelectableTags.Any(x => x.IsSelected);
            return areThereSelectedTags;
        }

        public override void Execute(object? parameter)
        {
            List<SelectableTagModel>? selectedTags = _tagListViewModel.SelectableTags.Where(x => x.IsSelected).ToList();
            foreach (SelectableTagModel selectedTag in selectedTags)
            {
                selectedTag.IsSelected = false;
            }
        }
    }
}
