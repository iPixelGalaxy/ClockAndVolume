﻿using HMUI;
using Zenject;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using System;

namespace Enhancements.UI
{
    [HotReload(RelativePathToLayout = @"..\Views\settings-navigation-view.bsml")]
    public class XSettingNavigationView : BSMLAutomaticViewController
    {
        public event Action<string, int> DidSelectSettingOption;

        [UIComponent("list")]
        protected CustomListTableData tableList;

        private XLoader _loader;

        [Inject]
        public void Construct(XLoader loader)
        {
            _loader = loader;
        }

        [UIAction("option-selected")]
        protected void OptionSelected(TableView _, int id)
        {
            DidSelectSettingOption?.Invoke(tableList.data[id].text.Split('\n')[0], id);
        }

        [UIAction("#post-parse")]
        protected void Parsed()
        {
            tableList.data.AddRange(new CustomListTableData.CustomCellInfo[]
            {
                new CustomListTableData.CustomCellInfo
                (
                    "Changelog\n",
                    "The changelog for Enhancements",
                    _loader.GetIcon("changelog")
                    
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Clock\n",
                    "Modify the Clock",
                    _loader.GetIcon("clock")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Timers\n",
                    "Create Reminders In Game!",
                    _loader.GetIcon("timer")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Breaktime\n",
                    "Get Information During Song Breaks!",
                    _loader.GetIcon("breaktime")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Volume\n",
                    "Change Specific Volume Settings",
                    _loader.GetIcon("volume")
                ),
                new CustomListTableData.CustomCellInfo
                (
                    "Mini Settings and Optidra\n",
                    "Miscellaneous Tweaks and Experimental Settings",
                    _loader.GetIcon("settings")
                )
            });
            tableList.tableView.ReloadData();
            SelectFirstCell();
        }

        public void SelectFirstCell()
        {
            tableList?.tableView.SelectCellWithIdx(0);
        }
    }
}