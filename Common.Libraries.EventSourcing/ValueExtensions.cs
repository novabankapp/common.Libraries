﻿using System;

namespace Common.Libraries.EventSourcing
{
    public static class ValueExtensions
    {
        public static void MustNotBeNull<T>(this Value<T> value) where T : Value<T>
        {
            if (value == null)
                throw new InvalidValueException(typeof(T), "cannot be null");
        }

        public static void MustBe<T>(this Value<T> value) where T : Value<T>
        {
            if (value == null)
                throw new InvalidValueException(typeof(T), "cannot be null");
        }
        
        public static T With<T>(this T instance, Action<T> update)
        {
            update(instance);
            return instance;
        }
    }
}