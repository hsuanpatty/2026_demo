//解決list-title區塊底線的問題 (不該出現底線的地方有底線))
function updateLastVisibleLink() {
  console.log('update');
  const listContainers = document.querySelectorAll('.list');

  listContainers.forEach((container) => {
    const listTitles = container.querySelectorAll('.list-title');
    let lastVisibleLink = null;
    listTitles.forEach((title) => {
      if (title.classList.contains('show')) {
        const link = title.querySelector('a');
        if (link) {
          link.style.borderBottom = '';
        }
      }
    });
    listTitles.forEach((title) => {
      if (title.classList.contains('show')) {
        lastVisibleLink = title.querySelector('a');
      }
    });
    if (lastVisibleLink) {
      lastVisibleLink.style.borderBottom = '0';
    }
  });
}

//最一開始執行一次
document.addEventListener('DOMContentLoaded', updateLastVisibleLink);
