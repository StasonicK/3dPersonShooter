using System;
using CodeBase.Data;

namespace CodeBase.Infrastructure
{
    public interface ISceneLoader
    {
        void Load(Scene scene, Action<Scene> onLoaded = null);
    }
}