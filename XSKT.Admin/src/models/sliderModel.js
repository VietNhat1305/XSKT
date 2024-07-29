const toJson = (item) => {
    return {
        _id: item._id,
        files: item.files,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        files: item.files,
    }
}

const baseJson = () => {
    return {
        _id: null,
        files: []
    }
}


export const  sliderModel = {
    toJson, fromJson, baseJson
}
