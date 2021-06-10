﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Collections.ObjectModel;
using Flurl.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ObjectPhotoUploader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class HomePage : Page
    {
        string username;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        ObservableCollection<string> hemispheres = new ObservableCollection<string>();
        ObservableCollection<int> zones = new ObservableCollection<int>();
        ObservableCollection<int> eastings = new ObservableCollection<int>();
        ObservableCollection<int> northings = new ObservableCollection<int>();
        ObservableCollection<int> contextNumbers = new ObservableCollection<int>();
        ObservableCollection<string> materials = new ObservableCollection<string>();
        ObservableCollection<string> categories = new ObservableCollection<string>();
        ObservableCollection<int> findNumbers = new ObservableCollection<int>();

        IList<SpatialContext> contexts;
        IList<MaterialCategory> materialCategories;
        IList<ObjectFind> objectFinds;

        SpatialContext selectedContext;
        ObjectFind selectedFind;

        StorageFolder selectedFolder;
        IReadOnlyList<StorageFile> fileList;

        API api = new API();
        public HomePage()
        {
            this.InitializeComponent();
            username = (string)localSettings.Values["username"];
            LoadContexts();
            LoadMaterialCategories();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values.Remove("AuthToken");
            Frame.Navigate(typeof(LoginPage));
        }

        private void setLoading(bool isLoading, string msg, bool append = false)
        {
            if (append)
            {
                status.Text += "\n" + msg;
            } else
            {
                status.Text = msg;
            }
            
            progbar.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void LoadContexts()
        {
            try
            {
                setLoading(true, "Loading Spatial Contexts..");
                contexts = await api.GetSpatialContextsAsync();
                HemisphereOptions();
                setLoading(false, "");
            }
            catch (FlurlHttpException ex)
            {
                string err = await ex.GetResponseStringAsync();
                setLoading(false, err);
            }
        }

        private async void LoadMaterialCategories()
        {
            try
            {
                setLoading(true, "Loading Material Categories...", true);
                materialCategories = await api.GetMaterialCategoriesAsync();
                materials = new ObservableCollection<string>(materialCategories.Select(x => x.material).Distinct());
                setLoading(false, "");
            } catch (FlurlHttpException ex)
            {
                string err = await ex.GetResponseStringAsync();
                setLoading(false, err);
            }
        }

        private void CategoryOptions()
        {
            IEnumerable<string> cats = materialCategories
                .Where(x => x.material == (string)material_cb.SelectedItem)
                .Select(x => x.category)
                .Distinct();
            categories.Clear();
            foreach (string c in cats)
            {
                categories.Add(c);
            }
            Bindings.Update();
        }

        private void HemisphereOptions()
        {
            IEnumerable<string> hems = contexts.Select(x => x.utm_hemisphere).Distinct();
            hemispheres.Clear();
            foreach (string h in hems)
            {
                hemispheres.Add(h);
            }

            if (hemispheres.Count() == 1)
            {
                utm_hemisphere.SelectedItem = hemispheres[0];
                utm_hemisphere.IsEnabled = false;
                ZoneOptions();
            }
        }

        private void ZoneOptions()
        {
            IEnumerable<int> distinct_zones = contexts
                .Where(x => x.utm_hemisphere == (string)utm_hemisphere.SelectedItem)
                .Select(x => x.utm_zone).Distinct();
            zones.Clear();
            foreach (int z in distinct_zones)
            {
                zones.Add(z);
            }
            if (zones.Count() == 1)
            {
                utm_zone.SelectedItem = zones[0];
                utm_zone.IsEnabled = false;
                EastingOptions();
            }
        }


        private void EastingOptions()
        {
            IEnumerable<int> distinct_eastings = contexts
                .Where(x => x.utm_hemisphere == (string)utm_hemisphere.SelectedItem &&
                x.utm_zone == (int)utm_zone.SelectedItem)
                .Select(x => x.area_utm_easting_meters).Distinct();
            eastings.Clear();
            foreach (int e in distinct_eastings)
            {
                eastings.Add(e);
            }

            if (eastings.Count() == 1)
            {
                utm_easting.SelectedItem = eastings[0];
                utm_easting.IsEnabled = false;
                northingOptions();
            }
        }

        private void northingOptions()
        {
            IEnumerable<int> distinct_northings = contexts
                .Where(x => x.utm_hemisphere == (string)utm_hemisphere.SelectedItem &&
                x.utm_zone == (int)utm_zone.SelectedItem &&
                x.area_utm_easting_meters == (int)utm_easting.SelectedItem)
                .Select(x => x.area_utm_northing_meters).Distinct();
            northings.Clear();
            foreach (int n in distinct_northings)
            {
                northings.Add(n);
            }
            if (northings.Count() == 1)
            {
                utm_northing.SelectedItem = northings[0];
                utm_northing.IsEnabled = false;
                CnOptions();
            }
        }

        private void CnOptions()
        {
            IEnumerable<int> distinctCN = contexts
                .Where(x => x.utm_hemisphere == (string)utm_hemisphere.SelectedItem &&
                x.utm_zone == (int)utm_zone.SelectedItem &&
                x.area_utm_easting_meters == (int)utm_easting.SelectedItem &&
                x.area_utm_northing_meters == (int)utm_northing.SelectedItem)
                .Select(x => x.context_number).Distinct();
            contextNumbers.Clear();
            foreach (int cn in distinctCN)
            {
                contextNumbers.Add(cn);
            }

            if (contextNumbers.Count() == 1)
            {
                context_number.SelectedItem = contextNumbers[0];
            }
        }

        private void utm_hemisphere_DropDownClosed(object sender, object e)
        {
            if (utm_hemisphere.SelectedIndex > -1)
            {
                ZoneOptions();
                eastings.Clear();
                northings.Clear();
                contextNumbers.Clear();
            }
        }

        private void utm_zone_DropDownClosed(object sender, object e)
        {
            if (utm_zone.SelectedIndex > -1)
            {
                EastingOptions();
                northings.Clear();
                contextNumbers.Clear();
            }
        }

        private void utm_easting_DropDownClosed(object sender, object e)
        {
            if (utm_easting.SelectedIndex > -1)
            {
                northingOptions();
            }
        }

        private void utm_northing_DropDownClosed(object sender, object e)
        {

            if (utm_northing.SelectedIndex > -1)
            {
                CnOptions();
            }
        }

        private void context_number_DropDownClosed(object sender, object e)
        {
            if (context_number.SelectedIndex > -1)
            {
                selectedContext = contexts.Single(x => {
                    return
                    x.utm_hemisphere == (string)utm_hemisphere.SelectedItem &&
                    x.utm_zone == (int)utm_zone.SelectedItem &&
                    x.area_utm_easting_meters == (int)utm_easting.SelectedItem &&
                    x.area_utm_northing_meters == (int)utm_northing.SelectedItem &&
                    x.context_number == (int)context_number.SelectedItem;
                    });
                loadFinds();
            }
        }

        private async void loadFinds()
        {
            try
            {
                setLoading(true, "Loading Finds...");
                objectFinds = await api.GetObjectFindsAsync(selectedContext);
                findNumbers = new ObservableCollection<int>(objectFinds.Select(x => x.find_number));
                Bindings.Update();
                setLoading(false, "");
            } catch (FlurlHttpException ex)
            {
                setLoading(false, ex.Message);
            }
        }

        private void object_find_DropDownClosed(object sender, object e)
        {
           if (object_find.SelectedIndex > -1)
            {
                selectedFind = objectFinds.Single(x => x.find_number == (int)object_find.SelectedItem);
                Bindings.Update();
            }
        }

        private async void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions
                    .FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                selectedFolder = folder;
                fileList = await selectedFolder.GetFilesAsync();
                watched_folder_list.ItemsSource = fileList;
                StorageLibraryChangeTracker photoTracker = selectedFolder.TryGetChangeTracker();
                photoTracker.Enable();
            }
            Bindings.Update();

        }

        private async void new_find_submit_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog newFindDialog = new ContentDialog
            {
                Title = "Create New Find",
                Content = string.Format(
                    "A new Find will be created in context {0} with the next available find number.",
                    selectedContext.ToString()),
                PrimaryButtonText = "Continue",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await newFindDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    setLoading(true, "Creating new find...");
                    ObjectFind newFind;
                    ObjectFindData data = new ObjectFindData(
                        selectedContext.utm_hemisphere,
                        selectedContext.utm_zone,
                        selectedContext.area_utm_easting_meters,
                        selectedContext.area_utm_northing_meters,
                        selectedContext.context_number,
                        (string)material_cb.SelectedItem,
                        (string)category_cb.SelectedItem,
                        director_notes_tb.Text
                        );

                    newFind = await api.CreateObjectFindAsync(data);
                    objectFinds.Insert(0, newFind);
                    findNumbers.Insert(0, newFind.find_number);
                    status.Text = string.Format(
                        "Created New Find in current context with number {0}",
                        newFind.find_number);
                    object_find.SelectedItem = newFind.find_number;
                    setLoading(false, "");
                } catch (FlurlHttpException ex)
                {
                    string errors = await ex.GetResponseStringAsync();
                    System.Diagnostics.Debug.WriteLine(errors);
                    setLoading(false, errors);
                }
            }
            
        }

        private async void fadeoutStatus(string message, int timeout = 3000)
        {
            status.Text = message;
            await System.Threading.Tasks.Task.Delay(timeout);
            status.Text = "";
        }

        private void material_cb_DropDownClosed(object sender, object e)
        {
            if (material_cb.SelectedIndex > -1)
            {
                CategoryOptions();
            }
        }
    }
}
