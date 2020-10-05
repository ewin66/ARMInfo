using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

using ARMInfo.WCF;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo
{

    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                ServiceClient sc = new ServiceClient();
                sc.InitService(new System.Net.IPAddress(new byte[] { 192, 168, 0, 100 }),
                    5005, 
                    "ARMInfo");//new byte[] { 10, 221, 0, 2 })
                
                var view = new MainWindow(sc);
                view.Show();
            }
            catch (Exception err)
            {
                ARMInfo.MainWindow.MayIGoOut = true;
                MessageBox.Show(err.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                Tracer.Append(err.Message);
                Current.Shutdown();
            }
        }
    }




    public partial class App : Application
    {


        ITracerException Tracer = new TracerException();


        private static void ShowView(IPCInfo pc = null)
        {
            var view = new MainWindow(pc);
            view.Show();
        }

        private void Application_Startup1(object sender, StartupEventArgs e)
        {
            try
            {

                var url = "&mac=" + (new SystemInfo().MacAddress);
                var list = Getway.LoadPcInfo(url);

                if (list.Count > 0)
                {
                    Tracer.Append($"Информация получена. Всего {list.Count} записей.");
                    var pcInfo = list?.First() ?? null;
                    if (pcInfo != null)
                    {
                        if (pcInfo.status == "IS_APPLY")
                        {
                            Getway.Log(pcInfo);//Данные по компьютеру уже приняты
                            Tracer.Append($"Данные по компьютеру уже приняты");
                            Current.Shutdown();// Проигнорировать и отправить SystemInfo
                        }
                        else if (pcInfo.status == "PROCESSING")
                        {
                            Getway.Log(pcInfo);
                            if (MessageBox.Show("Данные по вашему компьютеру находятся на обработке.\nВы хотите отредактировать какие либо значения?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ShowView(pcInfo); // данные есть, но статус не подтвержден
                                ARMInfo.MainWindow.MayIGoOut = true;             // сделать загрузку данных. (заполнение интерфейса) 
                            }
                            else
                            {
                                Current.Shutdown();
                            }
                        }
                        else { ShowView(); }
                    }
                    else
                    {
                        ShowView();// нет никаких данных о компе
                    }
                }
                else
                {
                    Tracer.Append($"Нет данных о компьютере. Открываю интерфейс пользователя.");
                    var newPc = new PCInfo();
                    newPc.SetUp(new SystemInfo(), new PersonalInfo());
                    Getway.Log(newPc);
                    ShowView(); // нет никаких данных о компе         
                }
            }
            catch (Exception err)
            {
                ARMInfo.MainWindow.MayIGoOut = true;
                MessageBox.Show(err.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);

                Tracer.Append(err.Message);
                Current.Shutdown();
            }
        }


    }







    public partial class App : Application
    {
        #region Fields
        ITracerException SenderException = new TracerException();
        string WorkDirectory = "";
        string currentLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // получаем текущее расположение

        #endregion

        #region INITIALIZATION & Startup
        private bool CheckWorkDirectory()
        {
            var path = $@"C:\Users\{Environment.UserName}\AppData\Local\{Assembly.GetExecutingAssembly().GetName().Name}";
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception err)
                {
                    SenderException.Append("Неудалось выполнить создание рабочего каталога приложения");
                    return false;
                }
            }
            WorkDirectory = path;
            return true;
        }
        private void KillOtherProcess()
        {
            var currentProcName = Assembly.GetExecutingAssembly().GetName().Name;
            var mustKillProcesses = Process.GetProcessesByName(currentProcName).Where(x => x.Id != Process.GetCurrentProcess().Id);
            foreach (var p in mustKillProcesses)
            {
                p.Kill();
            }

            Thread.Sleep(100);
        }
        private void SelfReplicate()
        {
            foreach (FileStream fs in Assembly.GetExecutingAssembly().GetFiles())
            {
                try
                {
                    File.Copy(fs.Name, WorkDirectory + $@"\{new FileInfo(fs.Name).Name}", true);
                }
                catch (IOException err)
                {
                    try
                    {
                        KillOtherProcess();
                        File.Copy(fs.Name, WorkDirectory + $@"\{new FileInfo(fs.Name).Name}", true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("не могу скопировать");
                        Tracer.Append(ex.Message);
                    }
                }
            }
        }
        private void Configuring()
        {
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true).SetValue("TechInfo", @"C:\Users\Alexander\AppData\Local\TechInfo\TechInfo.exe");
        }
        private void Init()
        {
            // если экземпляр запущен не из рабочего каталога -> копируем программу в рабочий каталог
            SelfReplicate();
            Configuring();
            string procName = WorkDirectory + $@"\{new FileInfo(Assembly.GetExecutingAssembly().Location).Name}";
            Process.Start(procName);// Запускаем программу в рабочем каталоге (она удалит этот экземпляр)
            App.Current.Shutdown();
        }

        private void Application_Startup2(object sender, StartupEventArgs e)
        {
#if !DEBUG
            if (CheckWorkDirectory())
            {
                // проверяем существует ли рабочий каталог или создаем его
                if (currentLocation.Equals(WorkDirectory))
                {
                    Thread.Sleep(100);
                    KillOtherProcess();
                    RunBL();
                }
                else
                {
                    Init();
                }
            }
            else
            {
                // Sender.Send();
                // если что-то пошло не так - передаем информацию на сервер
            }
#else
            RunBL();
#endif
        }

        #endregion

        private static void RunBL()
        {
            // логика приложения
            MessageBox.Show($"Готов выполнить порученое мне задание!\nId - {Process.GetCurrentProcess().Id}");
        }
    }
}
