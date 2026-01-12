mergeInto(LibraryManager.library, {

  _TriggerEvent: function(yandexMetrikaCounterId, eventDataUtf8) {
    const eventData = UTF8ToString(eventDataUtf8);
    
    try {
      const eventDataJson = eventData === '' ? undefined : JSON.parse(eventData);
      ym(yandexMetrikaCounterId, 'reachGoal', 'event', eventDataJson);
      console.log('[Yandex Metrika] Event sent: ', eventDataJson);

    } catch (e) {
      console.error('[Yandex Metrika] Event error: ', e.message);
    }

  },




});