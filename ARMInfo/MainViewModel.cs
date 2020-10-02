using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo
{

    //http://www.albahari.com/nutshell/linqkit.aspx
    public class MainViewModel : DependencyObject
    {
        public ISystemInfo SystemParameters { get; set; }

        public IPersonalInfo PersonalParameters { get; set; }

        public ObservableCollection<IPCInfo> ListPcInfo;

        public MainViewModel(ISystemInfo sp, IPersonalInfo pi)
        {
            this.SystemParameters = sp;
            this.PersonalParameters = pi;
            ListPcInfo = new ObservableCollection<IPCInfo>(Getway.LoadPcInfo());
        }

        public MainViewModel(IPCInfo pc)
        {
            OVDList = Getway.LoadOvdInfo();

            if (OVDList.Count == 0) { MainWindow.MayIGoOut = true; }

            ListPcInfo = new ObservableCollection<IPCInfo>(Getway.LoadPcInfo());
            this.SystemParameters = new SystemInfo();
            this.PersonalParameters = pc.GetPersonalInfo();
            SelectedOVD = OVDList.First(x => x.Id == pc.ovd);
            if (pc.department != null)
                SelectedDepartment = SelectedOVD.Departments?.First(x => x.Id == pc.department);
            var obj = OVD.AllObjects.First(x => x.id == pc.@object);
            SelectedAttestObject = obj;
            FilterText = PersonalParameters.InventoryNumber;
            SelectedPc = pc;
            IsDropDownPcList = false;
        }

        #region OVD

        public List<IOVDInfo> OVDList
        {
            get => (List<IOVDInfo>)GetValue(OVDListProperty);
            set { SetValue(OVDListProperty, value); }
        }

        public static readonly DependencyProperty OVDListProperty =
           DependencyProperty.Register("OVDList", typeof(List<IOVDInfo>), typeof(MainViewModel));

        public IOVDInfo SelectedOVD
        {
            get { return (IOVDInfo)GetValue(SelectedOVDProperty); }
            set
            {
                SetValue(SelectedOVDProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedOVDProperty = DependencyProperty.Register("SelectedOVD", typeof(IOVDInfo), typeof(MainViewModel),
             new PropertyMetadata(new PropertyChangedCallback(
                (d, ea) =>
                {
                    if (d is MainViewModel vm)
                    {
                        if (vm.SelectedDepartment != null)
                            vm.SelectedDepartment = null;
                    }
                }
                ))
            );

        public IDepartment SelectedDepartment
        {
            get { return (IDepartment)GetValue(SelectedDepartmentProperty); }
            set
            {
                SetValue(SelectedDepartmentProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedDepartmentProperty
            = DependencyProperty.Register("SelectedDepartment", typeof(IDepartment), typeof(MainViewModel),
               new PropertyMetadata(new PropertyChangedCallback(
                (d, ea) =>
                {
                    if (d is MainViewModel vm)
                    {
                        vm.PersonalParameters.Department = vm.SelectedDepartment;
                    }
                }
                ))
            );

        public IAttestObjectInfo SelectedAttestObject
        {
            get { return (IAttestObjectInfo)GetValue(SelectedAttestObjectProperty); }
            set
            {
                SetValue(SelectedAttestObjectProperty, value);
            }
        }
        public static readonly DependencyProperty SelectedAttestObjectProperty = DependencyProperty.Register("SelectedAttestObject", typeof(IAttestObjectInfo), typeof(MainWindow),
            new PropertyMetadata(new PropertyChangedCallback(
                (d, ea) =>
                {
                    if (d is MainViewModel vm)
                    {
                        vm.PersonalParameters.AttestObjectInfo = vm.SelectedAttestObject;
                    }
                }
                ))
            );

        #endregion



        public bool IsDropDownPcList
        {
            get { return (bool)GetValue(IsDropDownPcListProperty); }
            set { SetValue(IsDropDownPcListProperty, value); }
        }
        public static readonly DependencyProperty IsDropDownPcListProperty =
            DependencyProperty.Register("IsDropDownPcList", typeof(bool), typeof(MainViewModel));

        public List<IPCInfo> FilteredPcInfo
        {
            get { return (List<IPCInfo>)GetValue(FilteredPcInfoProperty); }
            set { SetValue(FilteredPcInfoProperty, value); }
        }
        public static readonly DependencyProperty FilteredPcInfoProperty =
            DependencyProperty.Register("FilteredPcInfo", typeof(List<IPCInfo>), typeof(MainViewModel));

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set
            {
                SetValue(FilterTextProperty, value);
            }
        }
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText",
                typeof(string), typeof(MainViewModel),

                new PropertyMetadata(new PropertyChangedCallback(
                (d, ea) =>
                {
                    if (d is MainViewModel vm)
                    {
                        if (!string.IsNullOrEmpty(vm.FilterText.Trim()))
                        {
                            vm.IsDropDownPcList = true;
                        }
                        vm.FilteredPcInfo = !string.IsNullOrEmpty(vm.FilterText.Trim())
                        ? vm.ListPcInfo?.Where(x => x.ovd == vm.SelectedOVD.Id)?.Where(x => x.inventory_number.Contains(vm.FilterText))?.ToList()
                        : vm.ListPcInfo?.Where(x => x.ovd == vm.SelectedOVD.Id).ToList();
                    }
                }
                ))
                );




        #region Отправить информацию
        private RelayCommand sendInfo;

        public IPCInfo SelectedPc
        {
            get { return (IPCInfo)GetValue(SelectedPcProperty); }
            set { SetValue(SelectedPcProperty, value); }
        }

        public static readonly DependencyProperty SelectedPcProperty =
            DependencyProperty.Register("SelectedPc", typeof(IPCInfo), typeof(MainViewModel));

        public RelayCommand SendInfo => sendInfo ?? (sendInfo = new RelayCommand(
        (parametr) =>
        {
            var currentPc = (IPCInfo)SelectedPc.Clone();


            if (currentPc.mac_address != SystemParameters.MacAddress && !string.IsNullOrEmpty(currentPc.mac_address))
            {
                var obj = SelectedOVD.AttestObjects.First(x => x.id == currentPc.@object);
                MessageBox.Show($"Данный инвентарный номер уже выбран пользователем: {currentPc.user}, {obj.Address}, Каб.№ {currentPc.room}");
                MessageBox.Show($"Свяжитесь с бухгалтерией своего ОВД для уточнения информации об инвентарном номере компьютера и повторите отправку позднее.\nПо всем вопросам обращайтесь по телефону 92-5-00 или 5520");
                MainWindow.MayIGoOut = true;
                App.Current.MainWindow.Close();
            }
            else
            {
                currentPc.SetUp(SystemParameters, PersonalParameters);
                if (string.IsNullOrEmpty(currentPc.user))
                {
                    MessageBox.Show($"Введите свое полное имя!");
                    return;
                }
                if (string.IsNullOrEmpty(currentPc.room))
                {
                    MessageBox.Show($"Значение \"Кабинет\" не может быть пустым!");
                    return;
                }

                var result = Getway.UpdatePcInfo(currentPc);
                if (result)
                {
                    MessageBox.Show($"Данные успешно отправлены!");
                    MainWindow.MayIGoOut = true;
                    App.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show($"Данные не отправлены. Повторите попытку позднее!\nПо всем вопросам обращайтесь по телефону 92-5-00 или 5520");
                    MainWindow.MayIGoOut = true;
                    App.Current.MainWindow.Close();
                }
            }
        }
        ));
        #endregion



    }


}
