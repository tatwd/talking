!(function () {
  var _data = {};
  var _xhr = new XMLHttpRequest();

  function _http(options) {
    _xhr.onreadystatechange = function (evt) {
      if (_xhr.readyState === 4 && _xhr.status === 200) {
        var res = JSON.parse(_xhr.responseText);
        if (options.success) options.success(res.data);
      }
    }
    _xhr.open(options.method, options.url)
    for (var k in options.headers) {
      if (options.headers.hasOwnProperty(k)) {
        _xhr.setRequestHeader(k, options.headers[k]);
      }
    }
    _xhr.send(options.data || null)
  }

  function _getComments() {
    _http({
      method: 'GET',
      url: _data.apiUri + '?post_url=' + location.href,
      success: function (data) {
        if (_data.render) {
          var html = ''
          data.forEach(i => {
            html += _data.render(i)
          });
          _data.el.querySelector('#comments').innerHTML = html;
        }
      }
    });
  }

  function _createComment(data) {
    _http({
      method: 'POST',
      url: _data.apiUri,
      data: JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json'
      },
      success: function (data) {
        _clearInput();
        console.log(data)
        _getComments(); // TODO: 不应重新渲染
      }
    });
  }

  function _clearInput() {
    _data.el.querySelector('form').reset();
  }

  function Talking(cb) {
    _data = cb();
    _data.el.innerHTML += `
      <form>
        用户名: <br><input name="owner.name" required/><br>
        邮箱: <br><input name="owner.email" required/><br>
        评论内容：<br><textarea name="htmlText" required></textarea><br>
        <button id="submitCommentBtn">发表</button>
      </form>
      <div id="comments"></div>
    `
    var inputOwnerName = _data.el.querySelector('input[name="owner.name"]');
    var inputOwnerEmail = _data.el.querySelector('input[name="owner.email"]');
    var inputHtmlText = _data.el.querySelector('textarea[name="htmlText"]');
    _data.el.addEventListener('click', function (evt) {
      evt.preventDefault();
      if (evt.target.id === 'submitCommentBtn') {
        var ownerName = inputOwnerName.value.trim();
        var ownerEmail = inputOwnerEmail.value.trim();
        var htmlText = inputHtmlText.value.trim();
        if (ownerName && ownerEmail && htmlText) {
          _createComment({
            owner: {
              name: ownerName,
              email: ownerEmail
            },
            postUrl: location.href,
            htmlText
          })
        } else {
          alert('请输入内容！');
        }
      }
    });
    console.log(_data);
    _getComments();
  }
  window.Talking = window.Talking || Talking;
})();
