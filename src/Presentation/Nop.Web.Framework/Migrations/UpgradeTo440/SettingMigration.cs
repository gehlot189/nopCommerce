﻿using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Configuration;

namespace Nop.Web.Framework.Migrations.UpgradeTo440
{
    [NopMigration("2020-06-10 00:00:00", "4.40.0", UpdateMigrationType.Settings)]
    [SkipMigrationOnInstall]
    public class SettingMigration: MigrationBase
    {
        /// <summary>Collect the UP migration expressions</summary>
        public override void Up()
        {
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            //do not use DI, because it produces exception on the installation process
            var settingService = EngineContext.Current.Resolve<ISettingService>();

            var externalAuthenticationSettings = settingService.LoadSetting<ExternalAuthenticationSettings>();
            if (!settingService.SettingExists(externalAuthenticationSettings, settings => settings.LogErrors))
            {
                externalAuthenticationSettings.LogErrors = false;

                settingService.SaveSetting(externalAuthenticationSettings);
            }

            var customerSettings = settingService.LoadSetting<CustomerSettings>();
            if (!settingService.SettingExists(customerSettings, settings => settings.ForceMultifactorAuthentication))
            {
                customerSettings.ForceMultifactorAuthentication = false;

                settingService.SaveSetting(customerSettings);
            }
        }

        public override void Down()
        {
            //add the downgrade logic if necessary 
        }
    }
}
