mergeInto(LibraryManager.library, {

   GetLanguage: function () {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  InitPlayer: function () {
    initPlayer();
  },

  InitYandex: function () {
    initYaSdk();
  },

  SaveExtern: function (data) {
    var dataString = UTF8ToString(data);
    var myObj = JSON.parse(dataString);
    player.setData(myObj);
  },

  LoadExtern: function () {
    player.getData().then(_data => {
      const myJson = JSON.stringify(_data);
      myGameInstance.SendMessage('Data', 'SetPlayerData', myJson);
    });
  },

  ShowInterstitialExtern: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          myGameInstance.SendMessage('Data', 'ContinuePlay');
        },
        onError: function(error) {
        }
      }
    });
  },

  ShowRevardedExtern: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          console.log('Rewarded!');
          myGameInstance.SendMessage('Data', 'Revard');
        },
        onClose: () => {
          console.log('Video ad closed.');
          myGameInstance.SendMessage('Data', 'ContinuePlay');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    });
  },

  Rate: function () {
    ysdk.feedback.canReview()
          .then(({ value, reason }) => {
              if (value) {
                  ysdk.feedback.requestReview()
                      .then(({ feedbackSent }) => {
                          console.log(feedbackSent);
                      })
              } else {
                  console.log(reason)
              }
          });
  },

  SetNewScore: function (value) {
    ysdk.getLeaderboards()
  .then(lb => {    
    // Без extraData
    lb.setLeaderboardScore('MaxScore', value);
  });
  }

});