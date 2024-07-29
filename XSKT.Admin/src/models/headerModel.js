const toJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
    }
}

const baseJson = () => {
    return {
        _id: null,
        file: null,
    }
}


export const  headerModel = {
    toJson, fromJson, baseJson
}
