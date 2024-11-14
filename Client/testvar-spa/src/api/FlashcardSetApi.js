import SortService from "../services/SortService";

const apiSetUrl = "http://localhost:3000/api/sets";

const FlashcardSetApi = {
    getFlashcardSets : function(){
        fetch(apiSetUrl)
            .then((response) => response.json())
            .then((result) => {
                return SortService.sortFlashcardSets(result.data, "name");
            })
            .catch((err) => {
                console.log(err.message);
            });
    },

    flashcardSetLoader: function (setId) {
        let url = '${apiSetUrl}/${setId}';
        console.log("Calling {url}");
        fetch(url)
            .then((response) => response.json())
            .then((result) => {
                return result
            })
            .catch((err) => {
                console.log(err.message);
            });
    }
}
export default FlashcardSetApi;