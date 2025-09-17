mergeInto(LibraryManager.library, {

  _SendEvent: function(yandexMetricaCounterId, eventNameUtf8, eventDataUtf8) {
    const eventName = UTF8ToString(eventNameUtf8);
    const eventData = UTF8ToString(eventDataUtf8);
    try {
      const eventDataJson = eventData === '' ? undefined : JSON.parse(eventData);
      ym(yandexMetricaCounterId, 'reachGoal', eventName, eventDataJson);
      console.log('Yandex Metrica: Event sent');

    } catch (e) {
      console.error('Yandex Metrica send event error: ', e.message);
    }

  },




});