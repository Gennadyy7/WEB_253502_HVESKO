document.addEventListener('DOMContentLoaded', function () {
    const containerId = 'product-list-container';
    const pagerLinkClass = '.page-link';

    function handlePaginationClick(event) {
        event.preventDefault();
        const url = new URL(event.target.href);
        const pageNo = url.searchParams.get('pageNo');
        const category = url.pathname.split('/')[2];

        fetch(`/Catalog/${category}?pageNo=${pageNo}`, {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(response => response.ok ? response.text() : Promise.reject(`Ошибка ${response.status}`))
            .then(html => {
                document.getElementById(containerId).innerHTML = html;
                bindPaginationEvents(); // Повторная привязка событий
            })
            .catch(error => console.error(`Не удалось загрузить данные: ${error}`));
    }

    function bindPaginationEvents() {
        if (window.location.pathname.startsWith('/Products/')) {
            document.querySelectorAll(pagerLinkClass).forEach(link => {
                link.addEventListener('click', handlePaginationClick);
            });
        }
    }

    bindPaginationEvents();
});
