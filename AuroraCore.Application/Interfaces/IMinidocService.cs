using System;
using AuroraCore.Application.DTOs;

namespace AuroraCore.Application.Interfaces 
{
    public interface IMinidocService {
      void Create(Guid ownerId, MinidocCreationParams creationParams);
    }
}