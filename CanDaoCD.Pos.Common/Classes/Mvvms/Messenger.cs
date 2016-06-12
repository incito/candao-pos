using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace CanDaoCD.Pos.Common.Classes.Mvvms
{
    /// <summary>
    /// The Messenger is a class allowing objects to exchange messages.
    /// </summary>
    public class Messenger : IMessenger
    {
        private static readonly object CreationLock = new object();
        private readonly object _registerLock = new object();
        private static IMessenger _defaultInstance;
        private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsOfSubclassesAction;
        private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsStrictAction;
        private bool _isCleanupRegistered;

        /// <summary>
        /// Gets the Messenger's default instance, allowing
        /// to register and send messages in a static manner.
        /// </summary>
        public static IMessenger Default
        {
            get
            {
                if (Messenger._defaultInstance == null)
                {
                    lock (Messenger.CreationLock)
                    {
                        if (Messenger._defaultInstance == null)
                            Messenger._defaultInstance = (IMessenger)new Messenger();
                    }
                }
                return Messenger._defaultInstance;
            }
        }

        /// <summary>
        /// Registers a recipient for a type of message TMessage. The action
        /// parameter will be executed when a corresponding message is sent.
        /// <para>Registering a recipient does not create a hard reference to it,
        /// so if this recipient is deleted, no memory leak is caused.</para>
        /// </summary>
        /// <typeparam name="TMessage">The type of message that the recipient registers
        /// for.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="action">The action that will be executed when a message
        /// of type TMessage is sent.</param>
        public virtual void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            this.Register<TMessage>(recipient, (object)null, false, action);
        }

        /// <summary>
        /// Registers a recipient for a type of message TMessage.
        /// The action parameter will be executed when a corresponding
        /// message is sent. See the receiveDerivedMessagesToo parameter
        /// for details on how messages deriving from TMessage (or, if TMessage is an interface,
        /// messages implementing TMessage) can be received too.
        /// <para>Registering a recipient does not create a hard reference to it,
        /// so if this recipient is deleted, no memory leak is caused.</para>
        /// </summary>
        /// <typeparam name="TMessage">The type of message that the recipient registers
        /// for.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="receiveDerivedMessagesToo">If true, message types deriving from
        /// TMessage will also be transmitted to the recipient. For example, if a SendOrderMessage
        /// and an ExecuteOrderMessage derive from OrderMessage, registering for OrderMessage
        /// and setting receiveDerivedMessagesToo to true will send SendOrderMessage
        /// and ExecuteOrderMessage to the recipient that registered.
        /// <para>Also, if TMessage is an interface, message types implementing TMessage will also be
        /// transmitted to the recipient. For example, if a SendOrderMessage
        /// and an ExecuteOrderMessage implement IOrderMessage, registering for IOrderMessage
        /// and setting receiveDerivedMessagesToo to true will send SendOrderMessage
        /// and ExecuteOrderMessage to the recipient that registered.</para>
        /// </param>
        /// <param name="action">The action that will be executed when a message
        /// of type TMessage is sent.</param>
        public virtual void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            this.Register<TMessage>(recipient, (object)null, receiveDerivedMessagesToo, action);
        }

        /// <summary>
        /// Registers a recipient for a type of message TMessage.
        /// The action parameter will be executed when a corresponding
        /// message is sent.
        /// <para>Registering a recipient does not create a hard reference to it,
        /// so if this recipient is deleted, no memory leak is caused.</para>
        /// </summary>
        /// <typeparam name="TMessage">The type of message that the recipient registers
        /// for.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="token">A token for a messaging channel. If a recipient registers
        /// using a token, and a sender sends a message using the same token, then this
        /// message will be delivered to the recipient. Other recipients who did not
        /// use a token when registering (or who used a different token) will not
        /// get the message. Similarly, messages sent without any token, or with a different
        /// token, will not be delivered to that recipient.</param>
        /// <param name="action">The action that will be executed when a message
        /// of type TMessage is sent.</param>
        public virtual void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            this.Register<TMessage>(recipient, token, false, action);
        }

        /// <summary>
        /// Registers a recipient for a type of message TMessage.
        /// The action parameter will be executed when a corresponding
        /// message is sent. See the receiveDerivedMessagesToo parameter
        /// for details on how messages deriving from TMessage (or, if TMessage is an interface,
        /// messages implementing TMessage) can be received too.
        /// <para>Registering a recipient does not create a hard reference to it,
        /// so if this recipient is deleted, no memory leak is caused.</para>
        /// </summary>
        /// <typeparam name="TMessage">The type of message that the recipient registers
        /// for.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="token">A token for a messaging channel. If a recipient registers
        /// using a token, and a sender sends a message using the same token, then this
        /// message will be delivered to the recipient. Other recipients who did not
        /// use a token when registering (or who used a different token) will not
        /// get the message. Similarly, messages sent without any token, or with a different
        /// token, will not be delivered to that recipient.</param>
        /// <param name="receiveDerivedMessagesToo">If true, message types deriving from
        /// TMessage will also be transmitted to the recipient. For example, if a SendOrderMessage
        /// and an ExecuteOrderMessage derive from OrderMessage, registering for OrderMessage
        /// and setting receiveDerivedMessagesToo to true will send SendOrderMessage
        /// and ExecuteOrderMessage to the recipient that registered.
        /// <para>Also, if TMessage is an interface, message types implementing TMessage will also be
        /// transmitted to the recipient. For example, if a SendOrderMessage
        /// and an ExecuteOrderMessage implement IOrderMessage, registering for IOrderMessage
        /// and setting receiveDerivedMessagesToo to true will send SendOrderMessage
        /// and ExecuteOrderMessage to the recipient that registered.</para>
        /// </param>
        /// <param name="action">The action that will be executed when a message
        /// of type TMessage is sent.</param>
        public virtual void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            lock (this._registerLock)
            {
                Type local_0 = typeof(TMessage);
                Dictionary<Type, List<Messenger.WeakActionAndToken>> local_1;
                if (receiveDerivedMessagesToo)
                {
                    if (this._recipientsOfSubclassesAction == null)
                        this._recipientsOfSubclassesAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
                    local_1 = this._recipientsOfSubclassesAction;
                }
                else
                {
                    if (this._recipientsStrictAction == null)
                        this._recipientsStrictAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
                    local_1 = this._recipientsStrictAction;
                }
                lock (local_1)
                {
                    List<Messenger.WeakActionAndToken> local_2;
                    if (!local_1.ContainsKey(local_0))
                    {
                        local_2 = new List<Messenger.WeakActionAndToken>();
                        local_1.Add(local_0, local_2);
                    }
                    else
                        local_2 = local_1[local_0];
                    WeakAction<TMessage> local_3 = new WeakAction<TMessage>(recipient, action);
                    Messenger.WeakActionAndToken local_4 = new Messenger.WeakActionAndToken()
                    {
                        Action = (WeakAction)local_3,
                        Token = token
                    };
                    local_2.Add(local_4);
                }
            }
            this.RequestCleanup();
        }

        /// <summary>
        /// Sends a message to registered recipients. The message will
        /// reach all recipients that registered for this message type
        /// using one of the Register methods.
        /// </summary>
        /// <typeparam name="TMessage">The type of message that will be sent.</typeparam>
        /// <param name="message">The message to send to registered recipients.</param>
        public virtual void Send<TMessage>(TMessage message)
        {
            this.SendToTargetOrType<TMessage>(message, (Type)null, (object)null);
        }

        /// <summary>
        /// Sends a message to registered recipients. The message will
        /// reach only recipients that registered for this message type
        /// using one of the Register methods, and that are
        /// of the targetType.
        /// </summary>
        /// <typeparam name="TMessage">The type of message that will be sent.</typeparam>
        /// <typeparam name="TTarget">The type of recipients that will receive
        /// the message. The message won't be sent to recipients of another type.</typeparam>
        /// <param name="message">The message to send to registered recipients.</param>
        public virtual void Send<TMessage, TTarget>(TMessage message)
        {
            this.SendToTargetOrType<TMessage>(message, typeof(TTarget), (object)null);
        }

        /// <summary>
        /// Sends a message to registered recipients. The message will
        /// reach only recipients that registered for this message type
        /// using one of the Register methods, and that are
        /// of the targetType.
        /// </summary>
        /// <typeparam name="TMessage">The type of message that will be sent.</typeparam>
        /// <param name="message">The message to send to registered recipients.</param>
        /// <param name="token">A token for a messaging channel. If a recipient registers
        /// using a token, and a sender sends a message using the same token, then this
        /// message will be delivered to the recipient. Other recipients who did not
        /// use a token when registering (or who used a different token) will not
        /// get the message. Similarly, messages sent without any token, or with a different
        /// token, will not be delivered to that recipient.</param>
        public virtual void Send<TMessage>(TMessage message, object token)
        {
            this.SendToTargetOrType<TMessage>(message, (Type)null, token);
        }

        /// <summary>
        /// Unregisters a messager recipient completely. After this method
        /// is executed, the recipient will not receive any messages anymore.
        /// </summary>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        public virtual void Unregister(object recipient)
        {
            Messenger.UnregisterFromLists(recipient, this._recipientsOfSubclassesAction);
            Messenger.UnregisterFromLists(recipient, this._recipientsStrictAction);
        }

        /// <summary>
        /// Unregisters a message recipient for a given type of messages only.
        /// After this method is executed, the recipient will not receive messages
        /// of type TMessage anymore, but will still receive other message types (if it
        /// registered for them previously).
        /// </summary>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        /// <typeparam name="TMessage">The type of messages that the recipient wants
        /// to unregister from.</typeparam>
        public virtual void Unregister<TMessage>(object recipient)
        {
            this.Unregister<TMessage>(recipient, (object)null, (Action<TMessage>)null);
        }

        /// <summary>
        /// Unregisters a message recipient for a given type of messages only and for a given token.
        /// After this method is executed, the recipient will not receive messages
        /// of type TMessage anymore with the given token, but will still receive other message types
        /// or messages with other tokens (if it registered for them previously).
        /// </summary>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        /// <param name="token">The token for which the recipient must be unregistered.</param>
        /// <typeparam name="TMessage">The type of messages that the recipient wants
        /// to unregister from.</typeparam>
        public virtual void Unregister<TMessage>(object recipient, object token)
        {
            this.Unregister<TMessage>(recipient, token, (Action<TMessage>)null);
        }

        /// <summary>
        /// Unregisters a message recipient for a given type of messages and for
        /// a given action. Other message types will still be transmitted to the
        /// recipient (if it registered for them previously). Other actions that have
        /// been registered for the message type TMessage and for the given recipient (if
        /// available) will also remain available.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages that the recipient wants
        /// to unregister from.</typeparam>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        /// <param name="action">The action that must be unregistered for
        /// the recipient and for the message type TMessage.</param>
        public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            this.Unregister<TMessage>(recipient, (object)null, action);
        }

        /// <summary>
        /// Unregisters a message recipient for a given type of messages, for
        /// a given action and a given token. Other message types will still be transmitted to the
        /// recipient (if it registered for them previously). Other actions that have
        /// been registered for the message type TMessage, for the given recipient and other tokens (if
        /// available) will also remain available.
        /// </summary>
        /// <typeparam name="TMessage">The type of messages that the recipient wants
        /// to unregister from.</typeparam>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        /// <param name="token">The token for which the recipient must be unregistered.</param>
        /// <param name="action">The action that must be unregistered for
        /// the recipient and for the message type TMessage.</param>
        public virtual void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            Messenger.UnregisterFromLists<TMessage>(recipient, token, action, this._recipientsStrictAction);
            Messenger.UnregisterFromLists<TMessage>(recipient, token, action, this._recipientsOfSubclassesAction);
            this.RequestCleanup();
        }

        /// <summary>
        /// Provides a way to override the Messenger.Default instance with
        /// a custom instance, for example for unit testing purposes.
        /// </summary>
        /// <param name="newMessenger">The instance that will be used as Messenger.Default.</param>
        public static void OverrideDefault(IMessenger newMessenger)
        {
            Messenger._defaultInstance = newMessenger;
        }

        /// <summary>
        /// Sets the Messenger's default (static) instance to null.
        /// </summary>
        public static void Reset()
        {
            Messenger._defaultInstance = (IMessenger)null;
        }

        /// <summary>
        /// Provides a non-static access to the static <see cref="M:GalaSoft.MvvmLight.Messaging.Messenger.Reset" /> method.
        /// Sets the Messenger's default (static) instance to null.
        /// </summary>
        public void ResetAll()
        {
            Messenger.Reset();
        }

        private static void CleanupList(IDictionary<Type, List<Messenger.WeakActionAndToken>> lists)
        {
            if (lists == null)
                return;
            lock (lists)
            {
                List<Type> local_0 = new List<Type>();
                foreach (KeyValuePair<Type, List<Messenger.WeakActionAndToken>> item_3 in (IEnumerable<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>)lists)
                {
                    List<Messenger.WeakActionAndToken> local_2 = new List<Messenger.WeakActionAndToken>();
                    foreach (Messenger.WeakActionAndToken item_0 in item_3.Value)
                    {
                        if (item_0.Action == null || !item_0.Action.IsAlive)
                            local_2.Add(item_0);
                    }
                    foreach (Messenger.WeakActionAndToken item_1 in local_2)
                        item_3.Value.Remove(item_1);
                    if (item_3.Value.Count == 0)
                        local_0.Add(item_3.Key);
                }
                foreach (Type item_2 in local_0)
                    lists.Remove(item_2);
            }
        }

        private static void SendToList<TMessage>(TMessage message, IEnumerable<Messenger.WeakActionAndToken> list, Type messageTargetType, object token)
        {
            if (list == null)
                return;
            foreach (Messenger.WeakActionAndToken weakActionAndToken in list.Take<Messenger.WeakActionAndToken>(list.Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>())
            {
                //IExecuteWithObject executeWithObject = weakActionAndToken.Action as IExecuteWithObject;
                //if (executeWithObject != null && weakActionAndToken.Action.IsAlive && weakActionAndToken.Action.Target != null && (messageTargetType == (Type)null || weakActionAndToken.Action.Target.GetType() == messageTargetType || messageTargetType.IsAssignableFrom(weakActionAndToken.Action.Target.GetType())) && (weakActionAndToken.Token == null && token == null || weakActionAndToken.Token != null && weakActionAndToken.Token.Equals(token)))
                //    executeWithObject.ExecuteWithObject((object)message);
            }
        }

        private static void UnregisterFromLists(object recipient, Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
        {
            if (recipient == null || lists == null || lists.Count == 0)
                return;
            lock (lists)
            {
                foreach (Type item_0 in lists.Keys)
                {
                    foreach (Messenger.WeakActionAndToken item_1 in lists[item_0])
                    {
                        //IExecuteWithObject local_2 = (IExecuteWithObject)item_1.Action;
                        //if (local_2 != null && recipient == local_2.Target)
                        //    local_2.MarkForDeletion();
                    }
                }
            }
        }

        private static void UnregisterFromLists<TMessage>(object recipient, object token, Action<TMessage> action, Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
        {
            Type key = typeof(TMessage);
            if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(key))
                return;
            lock (lists)
            {
                foreach (Messenger.WeakActionAndToken item_0 in lists[key])
                {
                    WeakAction<TMessage> local_2 = item_0.Action as WeakAction<TMessage>;
                    if (local_2 != null && recipient == local_2.Target && (action == null || action.Method.Name == local_2.MethodName) && (token == null || token.Equals(item_0.Token)))
                        item_0.Action.MarkForDeletion();
                }
            }
        }

        /// <summary>
        /// Notifies the Messenger that the lists of recipients should
        /// be scanned and cleaned up.
        /// Since recipients are stored as <see cref="T:System.WeakReference" />,
        /// recipients can be garbage collected even though the Messenger keeps
        /// them in a list. During the cleanup operation, all "dead"
        /// recipients are removed from the lists. Since this operation
        /// can take a moment, it is only executed when the application is
        /// idle. For this reason, a user of the Messenger class should use
        /// <see cref="M:GalaSoft.MvvmLight.Messaging.Messenger.RequestCleanup" /> instead of forcing one with the
        /// <see cref="M:GalaSoft.MvvmLight.Messaging.Messenger.Cleanup" /> method.
        /// </summary>
        public void RequestCleanup()
        {
            if (this._isCleanupRegistered)
                return;
            Dispatcher.CurrentDispatcher.BeginInvoke((Delegate)new Action(this.Cleanup), DispatcherPriority.ApplicationIdle, (object[])null);
            this._isCleanupRegistered = true;
        }

        /// <summary>
        /// Scans the recipients' lists for "dead" instances and removes them.
        /// Since recipients are stored as <see cref="T:System.WeakReference" />,
        /// recipients can be garbage collected even though the Messenger keeps
        /// them in a list. During the cleanup operation, all "dead"
        /// recipients are removed from the lists. Since this operation
        /// can take a moment, it is only executed when the application is
        /// idle. For this reason, a user of the Messenger class should use
        /// <see cref="M:GalaSoft.MvvmLight.Messaging.Messenger.RequestCleanup" /> instead of forcing one with the
        /// <see cref="M:GalaSoft.MvvmLight.Messaging.Messenger.Cleanup" /> method.
        /// </summary>
        public void Cleanup()
        {
            Messenger.CleanupList((IDictionary<Type, List<Messenger.WeakActionAndToken>>)this._recipientsOfSubclassesAction);
            Messenger.CleanupList((IDictionary<Type, List<Messenger.WeakActionAndToken>>)this._recipientsStrictAction);
            this._isCleanupRegistered = false;
        }

        private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            Type index = typeof(TMessage);
            Dictionary<Type, List<Messenger.WeakActionAndToken>> dictionary=null;
            if (this._recipientsOfSubclassesAction != null)
            {
                foreach (Type c in this._recipientsOfSubclassesAction.Keys.Take<Type>(this._recipientsOfSubclassesAction.Count<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>()).ToList<Type>())
                {
                    List<Messenger.WeakActionAndToken> weakActionAndTokenList = (List<Messenger.WeakActionAndToken>)null;
                    if (index == c || index.IsSubclassOf(c) || c.IsAssignableFrom(index))
                    {
                        bool lockTaken = false;
                        try
                        {
                            Monitor.Enter((object)(dictionary = this._recipientsOfSubclassesAction), ref lockTaken);
                            weakActionAndTokenList = this._recipientsOfSubclassesAction[c].Take<Messenger.WeakActionAndToken>(this._recipientsOfSubclassesAction[c].Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>();
                        }
                        finally
                        {
                            if (lockTaken)
                                Monitor.Exit((object)dictionary);
                        }
                    }
                    Messenger.SendToList<TMessage>(message, (IEnumerable<Messenger.WeakActionAndToken>)weakActionAndTokenList, messageTargetType, token);
                }
            }
            if (this._recipientsStrictAction != null)
            {
                bool lockTaken = false;
                try
                {
                    Monitor.Enter((object)(dictionary = this._recipientsStrictAction), ref lockTaken);
                    if (this._recipientsStrictAction.ContainsKey(index))
                    {
                        List<Messenger.WeakActionAndToken> list = this._recipientsStrictAction[index].Take<Messenger.WeakActionAndToken>(this._recipientsStrictAction[index].Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>();
                        Messenger.SendToList<TMessage>(message, (IEnumerable<Messenger.WeakActionAndToken>)list, messageTargetType, token);
                    }
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit((object)dictionary);
                }
            }
            this.RequestCleanup();
        }

        private struct WeakActionAndToken
        {
            public WeakAction Action;
            public object Token;
        }
    }
}
