using System;
using SilkyRing.Memory;
using SilkyRing.Utilities;

namespace SilkyRing.Services
{
    public class EventService
    {
        private readonly MemoryIo _memoryIo;
        private readonly HookManager _hookManager;
        
        public EventService(MemoryIo memoryIo, HookManager hookManager)
        {
            _memoryIo = memoryIo;
            _hookManager = hookManager;
        }
        
    }
}