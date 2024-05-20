﻿namespace ItTakesAVillage.Interfaces;

public interface IEventService<T>
{
    Task<bool> Create(T t);
    Task<List<T>> GetAll();
    Task<List<T>> GetAllOfGroup(object id);
    Task <bool> Delete(int eventId);
    Task <bool> Update(T t);
}
