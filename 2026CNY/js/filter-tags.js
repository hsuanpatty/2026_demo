filterSelection("all");

function filterSelection(c) {
    var $elements = $(".filterDiv");
    var className = c === "all" ? "" : c;

    $elements.removeClass("show");
    if (className) {
        $elements.filter("." + className).addClass("show");
    } else {
        $elements.addClass("show");
    }
}

//function filterSelection(c) {
//  var x, y, i;
//  x = document.getElementsByClassName("filterDiv");
//  if (c == "all") c = "";
//  for (i = 0; i < x.length; i++) {
//    w3RemoveClass(x[i], "show");
//    if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show");
//  }
//}

function w3AddClass(element, name) {
  var i, arr1, arr2;
    arr1 = element.className.split(" ");
    arr2 = name.split(" ");
    
  for (i = 0; i < arr2.length; i++) {
      if (arr1.indexOf(arr2[i]) == -1) {
          console.log(arr1 + " " + arr2[i] + " " + arr1.indexOf(arr2[i]));
        element.className += " " + arr2[i];
    }
  }
}

function w3RemoveClass(element, name) {
  var i, arr1, arr2;
  arr1 = element.className.split(" ");
  arr2 = name.split(" ");
  for (i = 0; i < arr2.length; i++) {
    while (arr1.indexOf(arr2[i]) > -1) {
      arr1.splice(arr1.indexOf(arr2[i]), 1);
    }
  }
  element.className = arr1.join(" ");
}

// Add active class to the current button (highlight it)
var btnContainer = document.getElementById("myBtnContainer");
var btns = btnContainer.getElementsByClassName("btn");
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function () {
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
}

// $(document).ready(function() {
//   var listElement = $('.list');

//   if (listElement.children().length === 0) {
//       $('.day').addClass('noshow');
//   }
// });


// $(document).ready(function() {
//   var listElement = $('.list');

//   if (listElement.is(':empty')) {
//       $('.day').addClass('noshow');
//   }
// });

   // 添加或移除.noshow类的函数
function toggleNoShowClass() {
    var listElements = document.querySelectorAll('.list');
    var dayElements = document.querySelectorAll('.day');

    for (var i = 0; i < listElements.length; i++) {
        var listElement = listElements[i];
        var dayElement = dayElements[i];

        if (listElement.innerHTML.trim() === '') {
            dayElement.classList.add('noshow');
        } else {
            dayElement.classList.remove('noshow');
        }
    }
}

// 页面加载时调用函数
window.addEventListener('load', toggleNoShowClass);