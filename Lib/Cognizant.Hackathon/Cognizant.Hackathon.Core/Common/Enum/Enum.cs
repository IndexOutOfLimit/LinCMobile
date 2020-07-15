using System;
using System.Diagnostics.CodeAnalysis;

namespace Cognizant.Hackathon.Core.Common.Enum
{   
    public enum MediaType
    {
        Image = 0
    }   
   
    public enum NavigationDirection
    {       
        Forward, 
        Backwards, 
        InPlace, 
    }
  
    public enum AppStatus
    {       
        Running, 
        JustStarted, 
        JustWokeUp, 
        GoingToSleep, 
        Sleeping, 
        Unknown
    }
   
    public enum CacheType
    {       
        NoCache,
        InMemoryCache,
        AzureCache,
        InProcessMemoryCache,
        OutOfProcessMemoryCache,
        RedisCache,
    }

    public enum Order
    {        
        Desc = 0, 
        Asc = 1, 
    }
  
    public enum ActivityStatus
    {       
        Initiated = 0, 
        Processing = 1, 
        Completed = 2, 
        Failed = 3, 
        Pending = 4, 
        Cancelled = 5, 
    }    

    public enum DeviceType
    {       
        Android, 
        iOS, 
        WindowsMobile,
        DotNet,
    }

    public enum LogSeverity
    {        
        Info,         
        Warning, 
        Error, 
    }
}