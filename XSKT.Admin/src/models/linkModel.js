const toJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        link:item.link,
        moTa: item.moTa,
        thuTu: item.thuTu
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        link:item.link,
        moTa: item.moTa,
        thuTu: item.thuTu
    }
}

const baseJson = () => {
    return {
        _id: null,
        file: null,
        link:null,
        moTa: null,
        thuTu: 0,
    }
}


export const  linkModel = {
    toJson, fromJson, baseJson
}
