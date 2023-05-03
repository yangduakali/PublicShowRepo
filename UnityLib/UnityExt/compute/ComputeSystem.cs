using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityExt.compute;

public abstract class ComputeSystem <T,TP>  where T : struct, IDataCompute where TP : struct, IDataComputeProperty{
    
    protected ComputeSystem(ComputeShader computeShader){
        ComputeShader = computeShader;
        _stride = Marshal.SizeOf<T>();
        Buffer = new ComputeBuffer(1, _stride);
        Data = Array.Empty<T>();
    }

    protected T[] Data { get; set; }
    protected readonly ComputeShader ComputeShader;
    protected ComputeBuffer Buffer;
    protected TP[] Properties = Array.Empty<TP>();
    protected int DataLenght;
    private readonly List<T> _tempData = new();
    private readonly List<TP> _tempProperty = new();
    private readonly int _stride;
    
    public abstract void Dispatch();
    
    protected int AddComputeData(T data, TP property){
        for (int i = 0; i < Data.Length; i++) {
            if(Data[i].IsActive) continue;
            Data[i] = InitializeData(data, i);
            Properties[i] = InitializeProperty(property, i); 
            return i;
        }
        
        var dataCount = Data.Length;
        _tempData.Clear();
        _tempProperty.Clear();
            
        for (int i = 0; i < dataCount; i++) {
            _tempData.Add(Data[i]);
            _tempProperty.Add(Properties[i]);
        }
        Data = new T[dataCount + 1];
        Properties = new TP[dataCount + 1];
        for (int i = 0; i < dataCount; i++) {
            Data[i] = _tempData[i];
            Properties[i] = _tempProperty[i];
        }
            
        Data[dataCount] = InitializeData(data, dataCount);
        Properties[dataCount] = InitializeProperty(property, dataCount);
        DataLenght = dataCount + 1;
            
        Buffer.Release();
        Buffer = new ComputeBuffer(DataLenght,_stride);
        OnDataIncremented(DataLenght);
        return dataCount;
    }
    
    protected virtual void OnDataIncremented(int dataCount){
    }
        
    protected virtual void RemoveDataCompute(int index){
        Data[index].IsActive = false;
    }

    public virtual void Release(){
        Buffer.Release();
    }
        
    protected virtual T InitializeData( T data, int index){
        data.IsActive = true;
        return data;
    }
    protected virtual TP InitializeProperty( TP property, int index){
        property.ID = index;
        return property;
    }
}

public interface IDataCompute{
    bool IsActive { get; set; }
}

public interface IDataComputeProperty{
    int ID { get; set; }
}