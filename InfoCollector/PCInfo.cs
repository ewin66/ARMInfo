using System;
using System.Linq;

using InfoCollector.PersonalInformation;

namespace InfoCollector.SystemInformation
{
    [Serializable]
    public class PCInfo : IPCInfo, ICloneable
    {
        public enum Status { IS_NOT_APPLY, PROCESSING, IS_APPLY };
        public int id { get; set; }
        public string name { get; set; } // имя компьютера из оборотки
        public string serial_number { get; set; } // серийник компа из оборотки
        public string inventory_number { get; set; } // инвентарник
        public bool isUnknownInventoryNumber { get; set; } // Пользователь не знает инвентарный номер
        public string price_start_up { get; set; } // из оборотки
        public string user { get; set; } // ФИО пользователя
        public string bailee { get; set; } // материально ответственное лицо из оборотки
        public string note { get; set; } // комментарий
                                         // объект аттестации (указывает пользователь)
        public string address_other { get; set; } // ??.. не нужен
        public string room { get; set; } // кабинет
        public int? ovd { get; set; }
        public int? department { get; set; }
        public int? @object { get; set; }
        public string mac_address { get; set; }
        public string ip_address { get; set; }
        public string host_name { get; set; }
        public string hard_serial_number { get; set; }
        public string hard_name { get; set; } // информация о всех ЖМД
        public string os { get; set; } // информация об ОС
        public string secret_net_studio_version { get; set; } // только версия
        public string crypto_pro_version { get; set; }  // только версия
        public string vipnet_client_version { get; set; } // только версия
        public string kaspersky_version { get; set; } // только версия
        public string status { get; set; } // флаг запрета изменений. (если он установлен, позвоните на номер 5527)
        public string date_apply { get; set; } // на сервере
        public string date_edit { get; set; } // на сервере

        public PCInfo() { }

        public IPersonalInfo GetPersonalInfo()
        {
            return new PersonalInfo
            {
                Room = this.room,
                AttestObjectInfo = OVD.AllObjects.First(x => x.id == this.@object),
                InventoryNumber = this.inventory_number,
                FullName = this.user
            };
        }

        public void SetUp(ISystemInfo si, IPersonalInfo pi)
        {
            user = pi.FullName;
            room = pi.Room;
            @object = pi.AttestObjectInfo.id;

            vipnet_client_version = si.VPNClientInfo.Version;
            kaspersky_version = si.AntivirusInfo.Version;
            crypto_pro_version = si.CryptoPROInfo.Version;
            secret_net_studio_version = si.DefenderNSDInfo.Version;

            ip_address = si.IpAddress;
            mac_address = si.MacAddress;
            host_name = si.HostName;
            hard_name = si.HDDInfoJson;
            os = si.OperationSystemJson;
            SetNullToEmptyStringMembers();
        }

        public override string ToString()
        {
            return inventory_number.ToString();
        }

        public object Clone()
        {
            return new PCInfo()
            {
                id = this.id,
                name = this.name,
                serial_number = this.serial_number,
                inventory_number = this.inventory_number,
                isUnknownInventoryNumber = this.isUnknownInventoryNumber,
                price_start_up = this.price_start_up,
                user = this.user,
                bailee = this.bailee,
                @object = this.@object,
                address_other = this.address_other,
                room = this.room,
                ovd = this.ovd,
                mac_address = this.mac_address,
                ip_address = this.ip_address,
                host_name = this.host_name,
                hard_serial_number = this.hard_serial_number,
                hard_name = this.hard_name,
                os = this.os,
                secret_net_studio_version = this.secret_net_studio_version,
                crypto_pro_version = this.crypto_pro_version,
                vipnet_client_version = this.vipnet_client_version,
                kaspersky_version = this.kaspersky_version,
                status = this.status,
                note = this.note,
                date_apply = this.date_apply,
                date_edit = this.date_edit
            };
        }

        private void SetNullToEmptyStringMembers()
        {
            this.serial_number = string.IsNullOrEmpty(this.serial_number) ? "" : this.serial_number;
            this.room = string.IsNullOrEmpty(this.room) ? "" : this.room;
            this.address_other = string.IsNullOrEmpty(this.address_other) ? "" : this.address_other;
            this.name = string.IsNullOrEmpty(this.name) ? "" : this.name;
            this.serial_number = string.IsNullOrEmpty(this.serial_number) ? "" : this.serial_number;
            this.price_start_up = string.IsNullOrEmpty(this.price_start_up) ? "" : this.price_start_up;
            this.user = string.IsNullOrEmpty(this.user) ? "" : this.user;
            this.bailee = string.IsNullOrEmpty(this.bailee) ? "" : this.bailee;
            this.note = string.IsNullOrEmpty(this.note) ? "" : this.note;
            this.inventory_number = string.IsNullOrEmpty(this.inventory_number) ? "" : this.inventory_number;
            this.hard_serial_number = string.IsNullOrEmpty(this.hard_serial_number) ? "" : this.hard_serial_number;
        }
    }


}
