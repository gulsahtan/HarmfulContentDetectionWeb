using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using HarmfulContentDetection.Authorization;

namespace HarmfulContentDetection.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class HarmfulContentDetectionNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Tenants,
                        L("Tenants"),
                        url: "Tenants",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.DataSet,
                        L("DataSet"),
                        url: "DataSet",
                        icon: "fas fa-database",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.ObjectDetectionImage,
                        L("ObjectDetectionImage"),
                        url: "ObjectDetectionImage/ObjectDetectionOnImage",
                        icon: "fas fa-images",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.ObjectDetectionVideo,
                        L("ObjectDetectionVideo"),
                        url: "ObjectDetectionVideo/ObjectDetectionOnVideo",
                        icon: "fas fa-video",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.VideoWatchingPlatform,
                        L("VideoWatchingPlatform"),
                        url: "VideoWatchingPlatform/VideoWatchingPlatform",
                        icon: "fas fa-video",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.VideoInsertPlatform,
                        L("VideoInsertPlatform"),
                        url: "VideoInsertPlatform/VideoInsertPlatform",
                        icon: "fas fa-video",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.ObjectDetectionRealTime,
                        L("ObjectDetectionRealTime"),
                        url: "ObjectDetectionRealTime/ObjectDetectionRealTime",
                        icon: "fas fa-photo-video",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                )                
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-theater-masks",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                            )
                )
             ;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, HarmfulContentDetectionConsts.LocalizationSourceName);
        }
    }
}
