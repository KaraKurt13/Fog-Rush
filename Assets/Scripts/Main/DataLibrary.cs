using Assets.Scripts.Collectibles;
using Assets.Scripts.Objects;
using Assets.Scripts.TileModifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataLibrary
{
    public Dictionary<GroundTypeEnum, TileBase> TileBases = new();

    public Dictionary<TileModifierTypeEnum, TileModifierBase> TileModifiers = new();

    public Dictionary<CollectibleTypeEnum, CollectibleType> CollectibleTypes = new();

    public Dictionary<MiscTypeEnum, TileBase> MiscTileBases = new();

    private void InitData()
    {
        foreach (GroundTypeEnum type in Enum.GetValues(typeof(GroundTypeEnum)))
        {
            var tileBase = Resources.Load<TileBase>($"TileBases/{type}");

            if (tileBase == null) continue;

            TileBases.Add(type, tileBase);
        }

        foreach (MiscTypeEnum type in Enum.GetValues(typeof(MiscTypeEnum)))
        {
            var tileBase = Resources.Load<TileBase>($"TileBases/Misc/{type}");

            if (tileBase == null) continue;

            MiscTileBases.Add(type, tileBase);
        }

        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in types)
        {
            if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(TileModifierBase)))
            {
                var instance = (TileModifierBase)Activator.CreateInstance(type);
                TileModifiers.Add(instance.Type, instance);
            }

            if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(CollectibleType)))
            {
                var instance = (CollectibleType)Activator.CreateInstance(type);
                CollectibleTypes.Add(instance.Type, instance);
                instance.Sprite = Resources.Load<Sprite>($"Sprites/Collectibles/{instance.Type}");
            }
        }
    }

    public DataLibrary()
    {
        InitData();
    }
}
