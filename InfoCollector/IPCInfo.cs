using System;

namespace InfoCollector.SystemInformation
{
    public interface IPCInfo : ICloneable
    {
        int id { get; set; }
        string name { get; set; } // имя компьютера из оборотки
        string serial_number { get; set; } // серийник компа из оборотки
        string inventory_number { get; set; } // инвентарник
        bool isUnknownInventoryNumber { get; set; } // Пользователь не знает инвентарный номер
        string price_start_up { get; set; } // из оборотки
        string user { get; set; } // ФИО пользователя
        string bailee { get; set; } // материально ответственное лицо из оборотки
        string note { get; set; } // комментарий
                                  // объект аттестации (указывает пользователь)
        string address_other { get; set; } // ??.. не нужен
        string room { get; set; } // кабинет
        int? ovd { get; set; }
        int? department { get; set; }
        int? @object { get; set; }
        string mac_address { get; set; }
        string ip_address { get; set; }
        string host_name { get; set; }
        string hard_serial_number { get; set; }
        string hard_name { get; set; } // информация о всех ЖМД
        string os { get; set; } // информация об ОС
        string secret_net_studio_version { get; set; } // только версия
        string crypto_pro_version { get; set; }  // только версия
        string vipnet_client_version { get; set; } // только версия
        string kaspersky_version { get; set; } // только версия
        string status { get; set; } // флаг запрета изменений. (если он установлен, позвоните на номер 5527)
        string date_apply { get; set; } // на сервере
        string date_edit { get; set; } // на сервере

        IPersonalInfo GetPersonalInfo();
    }


}
