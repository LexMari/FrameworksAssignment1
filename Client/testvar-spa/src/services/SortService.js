const sortAsc =(arr, field) => {
    return arr.sort((a, b) => {
        if (a[field] > b[field]) { return 1; }
        if (b[field] > a[field]) { return -1; }
        return 0;
    })
}

const sortDsc = (arr, field) => {
    return arr.sort((a, b) => {
        if (a[field] > b[field]) { return -1; }
        if (b[field] > a[field]) { return 1; }
        return 0;
    })
}

const SortService = {
    sortFlashcardSets: function(data, sort) {
        if (sort.direction === 'asc')
            return sortAsc(data, sort.field);
        else
            return sortDsc(data, sort.field)
    }
}
export default SortService;