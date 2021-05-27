using System;
using AuroraCore.Application.DTOs.Minidoc;

namespace AuroraCore.Application.Interfaces 
{
    public interface IMinidocService {
      void Create(Guid ownerId, MinidocCreationParams creationParams);
    }
}