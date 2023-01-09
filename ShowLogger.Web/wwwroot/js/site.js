// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

shows = (function () {
    return {
        init: function () {
            //oa_utilities.init_page($('#wAddRange'));
        },

        get_show_id: function () {
            return {
                ShowId: $('#hdnShowId').val()
            }
        },

        grid_init: function (e) {
            //e.query.set('userId', events.get_event_id().EventId);
        },

        creator_init: function (e) {
            //e.find('#EventId').val(events.get_event_id().EventId);
        },

        showAddNextEpisode: function (e) {
            var model = oa_utilities.getModel(e);
            return model.ShowTypeId === 1000;
        },

        showAddRange: function (e) {
            var model = oa_utilities.getModel(e);
            return model.ShowTypeId === 1000 && (!!model.SeasonNumber || !!model.EpisodeNumber);
        },

        addNextEpisode: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getShowsBaseURL('Show/AddNextEpisode'), {
                "showId": model.ShowId
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_saved_notification();
                    if (!!$('#gvShows')) {
                        oa_grid.reload_grid('gvShows');
                    }

                    if (!!$('#gvTVStats')) {
                        oa_grid.reload_grid('gvTVStats');
                    }
                }
            });
        },

        openAddRange: function (e) {
            var model = oa_utilities.getModel(e);
            oa_window.open('wAddRange', function () {
                oa_utilities.hide_validation_errors('frmAddRange');

                $('#AddRangeShowName').val(model.ShowName);
                $('#AddRangeSeasonNumber').val(model.SeasonNumber);
                $('#AddRangeStartEpisode').val(model.EpisodeNumber + 1);
                $('#AddRangeEndEpisode').val(model.EpisodeNumber + 2);
            });
        },

        postAddRange: function (e) {
            var formData = oa_utilities.getFormData($('#frmAddRange'));

            oa_utilities.ajaxPostData(getShowsBaseURL('Show/AddRange'), formData, function (data) {
                if (data.errors.length > 0) {
                    oa_utilities.show_validation_errors('frmAddRange', data.errors);
                } else {
                    oa_window.close('wAddRange');
                    oa_grid.reload_grid('gvShows');
                    oa_utilities.show_record_saved_notification();
                }
            });
        },

        btnCloseAddRange_Click: function (e) {
            oa_window.close('wAddRange');
        },

        btnAddRangeSeason_Click: function () {
            $('#AddRangeSeasonNumber').val(parseInt(($('#AddRangeSeasonNumber').val()) || 0) + 1);
        },

        btnAddRangeStartEpisodeAdd_Click: function () {
            $('#AddRangeStartEpisode').val(parseInt(($('#AddRangeStartEpisode').val()) || 0) + 1);
        },

        btnAddRangeStartEpisodeMinus_Click: function () {
            $('#AddRangeStartEpisode').val(parseInt(($('#AddRangeStartEpisode').val()) || 0) - 1);
        },

        btnAddRangeEndEpisodeAdd_Click: function () {
            $('#AddRangeEndEpisode').val(parseInt(($('#AddRangeEndEpisode').val()) || 0) + 1);
        },

        btnAddRangeEndEpisodeMinus_Click: function () {
            $('#AddRangeEndEpisode').val(parseInt(($('#AddRangeEndEpisode').val()) || 0) - 1);
        },

        delete: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getShowsBaseURL('Show/Delete'), {
                "showId": model.ShowId
            }, function (data) {
                if (data.errors.length > 0) {
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_deleted_notification();
                    oa_grid.reload_grid('gvShows');
                }
            });
        },

        btnToggleShowName_Click: function () {
            $('#divShowNameDDL').toggle();
            $('#divShowNameTxt').toggle();

            var text = $('#btnToggleShowName').html();

            if (text === 'New') {
                $('#btnToggleShowName').html('Old');
                $('#ddlShowNames').val('');
                $('#ddlShowNames').focus();
            } else {
                $('#btnToggleShowName').html('New');
                $('#ShowName').val('');
                $('#SeasonNumber').val('');
                $('#EpisodeNumber').val('');
                $('#ShowName').focus();
            }
        },

        btnAddSeason_Click: function () {
            $('#SeasonNumber').val(parseInt(($('#SeasonNumber').val()) || 0) + 1);
        },

        btnResetEpisode_Click: function () {
            $('#EpisodeNumber').val(1);
        },

        ddlShowNames_Change: function () {
            oa_utilities.ajaxGetData(getShowsBaseURL('Show/LoadShow'), { showName: $('#ddlShowNames').val() }, function (data) {
                $('#ShowName').val(data.showName);
                $('#SeasonNumber').val(data.seasonNumber);
                $('#EpisodeNumber').val(data.episodeNumber);
                $('#ShowTypeId').val(data.showTypeId);
            });
        }
    }
})();

watchlist = (function () {
    return {
        moveToShows: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getShowsBaseURL('Watchlist/MoveToShows'), {
                "watchlistId": model.WatchlistId
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_saved_notification();
                    oa_grid.reload_grid('gvWatchlist');
                }
            });
        },

        delete: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getShowsBaseURL('Watchlist/Delete'), {
                "watchlistId": model.WatchlistId
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_deleted_notification();
                    oa_grid.reload_grid('gvWatchlist');
                }
            });
        },
    }
})();

friends = (function () {
    return {
        showAcceptDenyFriendRequest: function (e) {
            var model = oa_utilities.getModel(e);
            return model.IsPending === true;
        },

        showDeleteFriend: function (e) {
            var model = oa_utilities.getModel(e);
            return model.IsPending === false;
        },

        acceptFriendRequest: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getBaseURL() + "/Home/Friend_AcceptFriendRequest", {
                "friendRequestId": model.Id
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_saved_notification();
                    oa_grid.reload_grid('gvFriends');
                }
            });
        },

        denyFriendRequest: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getBaseURL() + "/Home/Friend_DenyFriendRequest", {
                "friendRequestId": model.Id
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_saved_notification();
                    oa_grid.reload_grid('gvFriends');
                }
            });
        },

        deleteFriend: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getBaseURL() + "/Home/Friend_DeleteFriend", {
                "friendId": model.Id
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_saved_notification();
                    oa_grid.reload_grid('gvFriends');
                }
            });
        },
    }
})();

transactions = (function () {
    return {
        TransactionTypeId_Change: function () {
            var selected = parseInt($('#TransactionTypeId').val());
            var text = $('#btnToggleItem').html();
            var transactionId = parseInt($('#TransactionId').val());
            $('#ddlItems option').each((i, o) => {
                var model = $(o).attr('model');

                if (!!model) {
                    if (model.indexOf(selected.toString()) > 0) {
                        $(o).show();
                    } else {
                        $(o).hide();
                    }
                }
            })
            $('#btnToggleItem').show();
            switch (selected) {
                case 2000:  // ALIST_TICKET
                case 2001: { // TICKET
                    if (text === 'Old') {
                        $('#btnToggleItem').click();
                    }

                    if (transactionId === 0) {
                        $("#ShowId option:eq(1)").attr('selected', 'selected');
                        $('#CostAmt').focus();
                        $('#Item').val('Ticket');
                        $('#CostAmt').val('');
                        $('#DiscountAmt').val('');
                    }

                    $('#btnToggleItem').hide();
                    

                    break;
                }

                case 2002: { // PURCHASE
                    if (transactionId === 0) {
                        if (text === 'New') {
                            $('#btnToggleItem').click();
                        }
                        $("#ShowId option:eq(1)").attr('selected', 'selected');
                    } else {
                        if (text === 'Old') {
                            $('#btnToggleItem').click();
                        }
                    }

                    break;
                }

                case 2003: { // ALIST -- Needs to Select last A-list in dropdown
                    if (transactionId === 0) {
                        $("#ShowId option:eq(0)").attr('selected', 'selected');
                        $("#ddlItems option[model*='" + selected + "']").attr('selected', 'selected');
                        transactions.ddlItems_Change();
                        if (text === 'New') {
                            $('#btnToggleItem').click();
                        }
                    } else {
                        if (text === 'Old') {
                            $('#btnToggleItem').click();
                        }
                    }
                    
                    break;
                }
            }

            
        },

        btnToggleItem_Click: function () {
            $('#divItemTxt').toggle();
            $('#divItemDDL').toggle();

            var transactionId = parseInt($('#TransactionId').val());
            var text = $('#btnToggleItem').html();

            if (text === 'New') {
                $('#btnToggleItem').html('Old');

                if (transactionId === 0) {
                    $('#ddlItems').val('');
                    $('#ddlItems').focus();
                }
            } else {
                $('#btnToggleItem').html('New');
                if (transactionId === 0) {
                    $('#Item').val('');
                    $('#CostAmt').val('');
                    $('#DiscountAmt').val('');
                    $('#Item').focus();
                }
            }
        },

        ddlItems_Change: function () {
            var model = oa_dropdownlist.getModel($('#ddlItems :selected'));

            $('#Item').val(model.item);
            $('#CostAmt').val(model.costAmt);
        },

        init_editor: function (e) {

            setTimeout(function () {
                var transactionId = parseInt($('#TransactionId').val());
                transactions.TransactionTypeId_Change();
                if (transactionId != 0) {
                    $('#btnToggleItem').hide();
                }
            }, 50)
        },

        delete: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getShowsBaseURL('Transaction/Delete'), {
                "transactionId": model.TransactionId
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_deleted_notification();
                    oa_grid.reload_grid('gvTransactions');
                }
            });
        },
    }
})();

books = (function () {
    return {
        delete: function (e) {
            var model = oa_utilities.getModel(e);

            oa_utilities.ajaxPostData(getBooksBaseURL('Book/Delete'), {
                "bookId": model.BookId
            }, function (data) {
                if (data.errors.length > 0) {
                    console.log('error: ', data.errors);
                    oa_utilities.show_record_error_notification(data.errors[0].errorMessage);
                } else {
                    oa_utilities.show_record_deleted_notification();
                    oa_grid.reload_grid('gvBooks');
                }
            });
        },
    }
})();

areas = (function () {
    return {
        btnSetShowsAreaAsDefault_Click: function () {
            oa_utilities.ajaxPostData(getCommonBaseURL('User/SetDefaultArea'), {
                "area": "Shows"
            }, function (data) {
                $('#btnSetShowsAreaAsDefault').hide();
                oa_utilities.show_record_saved_notification();
            });
        },

        btnSetBooksAreaAsDefault_Click: function () {
            oa_utilities.ajaxPostData(getCommonBaseURL('User/SetDefaultArea'), {
                "area": "Books"
            }, function (data) {
                $('#btnSetBooksAreaAsDefault').hide();
                oa_utilities.show_record_saved_notification();
            });
        }
    }
})();

account = (function () {
    return {
        login: function (e) {
            var form = $('#frmLogin');
            var formData = oa_utilities.getFormData(form);

            var loginUrl = getCommonBaseURL() + 'Account/LoginFromPage';
            ss_utilities.ajaxPostData(loginUrl, formData, function (data) {
                if (data.errors.length > 0) {
                    oa_utilities.show_validation_errors('frmLogin', data.errors);

                } else {
                    oa_utilities.hide_validation_errors('frmLogin');
                    window.location.href = data.returnUrl;
                }
            });
        },

        register: function (e) {
            var form = $('#frmRegister');
            var formData = oa_utilities.getFormData(form);

            var registerUrl = getCommonBaseURL() + 'Account/RegisterFromPage';
            oa_utilities.ajaxPostData(registerUrl, formData, function (data) {
                if (data.errors.length > 0) {
                    oa_utilities.show_validation_errors('frmRegister', data.errors);

                } else {
                    oa_utilities.hide_validation_errors('frmRegister');
                    window.location.href = data.returnUrl;
                }
            });
        },
    }
})();

getBaseURL = function () { return $("#hdnRootPath").val(); }
getCommonBaseURL = function (action) { return getBaseURL() + "/Common/" + action; }
getShowsBaseURL = function (action) { return getBaseURL() + "/Shows/" + action }
getBooksBaseURL = function (action) { return getBaseURL() + "/Books/" + action }

oa_window = (function () {
    return {
        open: function (windowId, e) {
            var w = $('#' + windowId);
            var title = w.attr("windowTitle");
            $("#oaWindowTitle").html(title);
            var postWindowFunc = w.attr("postWindowFunc");
            var url = w.attr('windowPartial');
            $('#oaWindowSave').prop("onclick", null).off("click")
            
            oa_utilities.ajaxPost(url, function (data) {
                $('#oaWindowBody').html(data);

                if (!!e) {
                    e();
                }

                oa_utilities.init_page($('#oaWindowBody'));

                $('#oaWindowSave').prop("onclick", null).off("click");
                if (!!postWindowFunc) {
                    var postFunc = window[postWindowFunc.split(".")[0]][postWindowFunc.split(".")[1]];
                    $('#oaWindowSave').bind('click', postFunc);
                }
                
                $("#oaWindow").modal('show');

                $('#oaWindow').on('hidden.bs.modal', function () {
                    $("#oaWindowTitle").html('');
                    $('#oaWindowBody').html('');
                    $('#oaWindowSave').prop("onclick", null).off("click")
                })
            });

            
        },

        close: function (windowId) {
            $("#oaWindow").modal('toggle');
        }
    }
})();

oa_grid = (function () {
    return {
        open_creator: function (gridName) {
            //var grid = $('#' + gridName).children()[0]);
            var grid = $('#' + gridName).find('table');
            var url = grid.attr('creator_view');
            var title = grid.attr("creator_title");
            var createUrl = grid.attr("creator_create_url");

            var parentName = grid.attr("parent_name");
            var parentValue = grid.attr("parent_val_func");

            var subParentName = grid.attr("sub_parent_name");
            var subParentValue = grid.attr("sub_parent_val");

            $("#gridModelEditorTitle").html(title);

            var preCreateFunc = grid.attr("pre_creator_func");
            if (!!preCreateFunc) {
                var func = window[preCreateFunc.split(".")[0]][preCreateFunc.split(".")[1]];
                func($('#gridModelEditorBody'));
            }

            oa_utilities.ajaxPost(url, function (data) {
                $('#gridModelEditorBody').html(data);

                oa_utilities.init_page($('#gridModelEditorBody'));

                var createFunc = grid.attr("creator_func");
                if (!!createFunc) {
                    var func = window[createFunc.split(".")[0]][createFunc.split(".")[1]];
                    func($('#gridModelEditorBody'));
                }

                $('#gridModelEditorSave').prop("onclick", null).off("click");
                $('#gridModelEditorSave').on('click', function () {
                    var formData = oa_utilities.getFormData($('#gridModelEditorBody').find('form'), parentName, parentValue, subParentName, subParentValue);

                    oa_utilities.ajaxPostData(createUrl, formData, function (data) {
                        if (data.errors.length > 0) {
                            oa_utilities.show_grid_validation_errors(data.errors);
                        } else {
                            $('#gridModelEditor').modal('toggle');
                            oa_grid.reload_grid(gridName);
                            oa_utilities.show_record_saved_notification();
                        }
                    });

                });
                $("#gridModelEditor").modal('show');
            });
        },

        open_editor: function (e) {
            var grid = oa_utilities.getGrid(e);
            var row = oa_utilities.getRow(e);
            var url = grid.attr("editor_view");
            var title = grid.attr("editor_title");
            var model = JSON.parse(row.attr("model"));
            var updateUrl = grid.attr("editor_update_url");
            var idProp = grid.attr("id_prop");
            var gridName = $(grid[0].parentElement.parentElement).attr("id");

            var parentName = grid.attr("parent_name");
            var parentValue = grid.attr("parent_val_func");

            var subParentName = grid.attr("sub_parent_name");
            var subParentValue = grid.attr("sub_parent_val");

            $("#gridModelEditorTitle").html(title);

            var modelData = { model: model };

            var preEditorFunc = grid.attr("pre_editor_func");
            if (!!preEditorFunc) {
                var func = window[preEditorFunc.split(".")[0]][preEditorFunc.split(".")[1]];
                func($('#gridModelEditorBody'));
            }

            oa_utilities.ajaxPostData(url, modelData, function (data) {
                $('#gridModelEditorBody').html(data);

                oa_utilities.init_page($('#gridModelEditorBody'));

                var updateFunc = grid.attr("editor_func");
                if (!!updateFunc) {
                    var func = window[updateFunc.split(".")[0]][updateFunc.split(".")[1]];
                    func($('#gridModelEditorBody'));
                }

                $('#gridModelEditorSave').prop("onclick", null).off("click");
                $('#gridModelEditorSave').on('click', function () {
                    var formData = oa_utilities.getFormData($('#gridModelEditorBody').find('form'), parentName, parentValue, subParentName, subParentValue);

                    oa_utilities.ajaxPostData(updateUrl, formData, function (data) {
                        if (data.errors.length > 0) {
                            oa_utilities.show_grid_validation_errors(data.errors);
                        } else {
                            $('#gridModelEditor').modal('toggle');
                            oa_grid.update_row(gridName, data.data, idProp);
                            oa_utilities.show_record_saved_notification();
                        }
                    });
                });
                $("#gridModelEditor").modal('show');
            });
        },

        reload_grid: function (gridName) {
            var partial = $('#' + gridName + '_partial');

            var data = { actionUrl: partial.attr("partial") };
            var url = $('#hlAjaxGrid').val();

            oa_utilities.ajaxGetData(url, data, function (data) {
                partial.html(data);
                partial.find(".mvc-grid").each(function () {
                    new MvcGrid(this);
                });
            });
        },

        update_row: function (gridName, model, idProp) {
            if (!!idProp) {
                var rows = $('#' + gridName + ' tbody tr');

                rows.each((r) => {
                    var rowModel = JSON.parse($(rows[r]).attr('model'));

                    if (model[idProp[0].toLowerCase() + idProp.substr(1)] === rowModel[idProp]) {
                        var modelJson = JSON.stringify(model, function (key, value) {
                            if (value && typeof value === 'object') {
                                var replacement = {};
                                for (var k in value) {
                                    if (Object.hasOwnProperty.call(value, k)) {
                                        replacement[k && k.charAt(0).toUpperCase() + k.substring(1)] = value[k];
                                    }
                                }
                                return replacement;
                            }
                            return value;
                        });

                        $(rows[r]).attr('model', modelJson);

                        for (var key in model) {
                            var td = $(rows[r]).find('td.' + gridName + '_' + key[0].toUpperCase() + key.substr(1).toLowerCase());

                            if (!!td) {
                                var propVal = model[key];

                                if (!!propVal) {
                                    if (key.toLowerCase().includes("date") && !key.toLowerCase().includes("time")) {
                                        propVal = moment(propVal).format('M/D/YYYY');
                                    }
                                    td.html(propVal);
                                }
                            }
                        }

                        //var tds = $(rows[r]).find('td');

                        //tds.each((t) => {

                        //});
                    }
                });
            } else {
                var page = $('#' + gridName).find('.mvc-grid-pager .active').data('page')
                $('#' + gridName).parent().attr('page', page);

                oa_grid.reload_grid(gridName);
            }

        },

        get_id_from_button(e) {
            return $(e).parent().parent().parent().parent().parent().attr('id');
        },

        create_grid_predicate: function (gridName, clearProp) {
            var predicates = new MvcGrid(document.querySelector('#' + gridName)).columns.map((e) => {
                return !!!clearProp && clearProp != e.name ? oa_utilities.createPredicate(e.name, e.filter) : "";
            }).filter(function (v) { return v !== '' });

            return predicates.join(" && ");
        },

        get_grid_sort: function (gridName) {

            var columns = new MvcGrid(document.querySelector('#' + gridName)).columns;

            var sort = {
                order: "",
                orderProp: ""
            }

            for (var i = 0; i < columns.length; ++i) {
                if (!!columns[i].sort) {
                    if (!!columns[i].sort.order) {
                        sort.order = columns[i].sort.order.toUpperCase();
                        sort.orderProp = oa_utilities.nameToProperty(columns[i].name);
                        break;
                    }
                }
            }

            //var sorts = new MvcGrid(document.querySelector('#' + gridName)).columns.map((e) => {
            //    //console.log(e.name, "e.sort: " + !!e.sort + ", e.order: " + e.sort.order);
            //    return !!e.sort && e.sort.order !== '' ? { orderProp: oa_utilities.nameToProperty(e.name), order: e.order.toUpperCase() } : "";
            //});//.filter(function (v) { return v.orderProp !== '' });

            console.log(sort);

            return sort;
        },

        set_server_filter(e, name) {
            var url = getCommonBaseURL() + 'Grid/SaveServerFilter';
            var orderProp = $('#' + e).parent().attr('orderProp');
            var order = $('#' + e).parent().attr('order');

            order = !!order ? order.toUpperCase() : '';
            orderProp = !!orderProp ? oa_utilities.nameToProperty(orderProp) : '';

            var data = {
                name: e,
                filter: {
                    count: $('#' + e).parent().parent().find('[name=numCount]').val(),
                    offset: $('#' + e).parent().parent().find('[name=numOffset]').val(),
                    filter: oa_grid.create_grid_predicate(e, name),
                    order: order,
                    orderProp: orderProp
                }
            }

            oa_utilities.ajaxPostData(url, data, function (data) {
                (new MvcGrid(document.querySelector('#' + e))).reload();
            });
        },

        set_grid_total(e) {
            var url = getCommonBaseURL() + 'Grid/GetTotal';

            var data = {
                name: e.name
            }

            oa_utilities.ajaxPostData(url, data, function (data) {
                $('#' + e.name).find('.oa_grid_total').html(data.counts.filtered + " of " + data.counts.total + " records")
            });
        },

        apply_filter(e) {
            oa_grid.set_server_filter(e, '');
        },

        clear_filter(e) {
            $('#' + e).parent().parent().find('[name=numCount]').val('500');
            $('#' + e).parent().parent().find('[name=numOffset]').val('0');
            oa_grid.reload_grid(e);
        }
    }
})();

oa_tabs = (function () {
    return {
        open_tab: function (e) {
            var tab = oa_utilities.getTab(e);

            $("." + tab.tabGroup).removeClass("active-tab");
            $("." + tab.tabGroup).addClass("non-active-tab");
            $("[tab=" + tab.openTab + "]").removeClass("non-active-tab");
            $("[tab=" + tab.openTab + "]").addClass("active-tab");

            $("[tabgroup=" + tab.tabGroup + "]").removeClass("active");
            $("[opentab=" + tab.openTab + "]").addClass("active");
            $("[closetab=" + tab.openTab + "]").addClass("active");

            var partialUrl = $(e).attr("partial");

            if (!!partialUrl) {
                var url = $('#hlTabPartial').val();
                var data = { partialUrl: partialUrl };
                oa_utilities.ajaxGetData(url, data, function (data) {
                    $("[tab=" + tab.openTab + "]").html(data);

                    oa_utilities.init_page($("[tab=" + tab.openTab + "]"));

                    $(e).attr("partial", "");
                });
            } else {
                $("[tab=" + tab.openTab + "]").find('.mvc-grid').each((i, g) => {
                    oa_grid.reload_grid(g.id);
                })
            }

        },

        add_tab: function (tabGroup, id, name, view) {
            var alreadyOpen = $('#' + tabGroup).find("button[opentab='" + tabGroup + "-" + id.replace(/\s/g, '') + "']").length > 0;

            console.log('alreadyOpen:', alreadyOpen);
            if (!alreadyOpen) {
                var div = $("<div></div>");

                var tab = $("<button></button>");
                tab.addClass("tablinks tab_button_left");
                tab.attr("onClick", "oa_tabs.open_tab(this)");
                tab.attr("opentab", tabGroup + "-" + id.replace(/\s/g, ''));
                tab.attr("partial", view);
                tab.attr("tabGroup", tabGroup + "-tabs");
                tab.html(name);

                div.append(tab);

                var close = $("<button></button>");
                close.addClass("tablinks tab_button_right closetab");
                close.attr("closetab", tabGroup + "-" + id.replace(/\s/g, ''));
                close.html('<span class="fas fa-times-circle"></span>');
                close.attr("onClick", "oa_tabs.delete_tab('" + tabGroup + "','" + id.replace(/\s/g, '') + "')");
                close.attr("tabGroup", tabGroup + "-tabs");
                div.append(close);

                var body = $("<div></div>");
                body.addClass("non-active-tab tabcontent " + tabGroup + "-tabs");
                body.attr("tab", tabGroup + "-" + id.replace(/\s/g, ''));

                $("button[tabgroup='" + tabGroup + "-tabs']").parent('.tab').append(div);
                //$('#' + tabGroup).find(".tab").append(div);
                $('#' + tabGroup).append(body);

                oa_tabs.open_tab(tab.get());
            } else {
                oa_tabs.open_tab($($('#' + tabGroup).find("button[opentab='" + tabGroup + "-" + id.replace(/\s/g, '') + "']")[0]));
            }


        },

        delete_tab: function (tabGroup, id) {
            var button = $('#' + tabGroup).find('button[opentab="' + tabGroup + "-" + id.replace(/\s/g, '') + '"]');
            var close = $('#' + tabGroup).find('button[closetab="' + tabGroup + "-" + id.replace(/\s/g, '') + '"]');
            var div = $('#' + tabGroup).find('div[tab="' + tabGroup + "-" + id.replace(/\s/g, '') + '"]');

            if (button.hasClass('active')) {
                oa_tabs.open_tab($('#' + tabGroup + ' .tab').children()[0]);
            }

            button.remove();
            close.remove();
            div.remove();

            //$('#tbPlayer').find('button[opentab="tbPlayer-170"]')
            //$('#tbPlayer').find('div[tab="tbPlayer-170"]')
        }
    }
})();

oa_dropdownlist = (function () {
    return {
        init_ddl: function (e) {
            var select = $(e);

            var parentName = select.attr('parent_name');
            var parentFunc = select.attr('parent_func');
            var valueListUrl = select.attr('value_list_url');
            var dataTextName = select.attr('data_text_name');
            var dataTextValue = select.attr('data_text_value');
            var defaultValue = select.attr('defaultValue');
            var emptyOption = select.attr('emptyOption');

            if (!!parentFunc) {
                var func = window[parentFunc.split(".")[0]][parentFunc.split(".")[1]];
                var parentValue = func();

                valueListUrl += '?' + parentName + '=' + parentValue;
            }

            if (!!emptyOption) {
                var newOption = $('<option value="">' + emptyOption + '</option>');
                select.append(newOption);
            }

            oa_utilities.ajaxGet(valueListUrl, function (data) {
                if (!!data && data.length > 0) {
                    $.each(data, function () {
                        var newOption = $('<option value="' + this[dataTextValue] + '">' + this[dataTextName] + '</option>');
                        newOption.attr('model', JSON.stringify(this));
                        select.append(newOption);
                    });

                    if (!!defaultValue) {
                        select.find('option[value="' + defaultValue + '"]').prop("selected", true);
                    }
                }
            });
        },

        on_change: function (e) {
            console.log('oa_dropdownlist.on_change', $(e));
        },

        get_selected_option: function (e) {
            return $("#" + e + ' option:selected');
        },

        getModel: function (e) {

            return JSON.parse(e.attr("model"));
        }
    }
})();

oa_utilities = (function () {
    $(document).on('show.bs.modal', '.modal', function () {
        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        setTimeout(function () {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);
    });

    $(document).ajaxSend(function (event, request, settings) {
        $('#divLoading').modal()
    });

    $(document).ajaxComplete(function (event, request, settings) {
        setTimeout(function () {
            $('#divLoading').modal('hide');
        }, 500)
    });

    return {
        getTab: function (e) {
            return {
                tabGroup: $(e).attr("tabGroup"),
                openTab: $(e).attr("openTab")
            }
        },

        getGrid: function (e) {
            return $(e.parentElement.parentElement.parentElement.parentElement);
        },

        getRow: function (e) {
            return $(e.parentElement.parentElement);
        },

        getModel: function (e) {
            var row = oa_utilities.getRow(e);
            return JSON.parse(row.attr("model"));
        },

        ajaxPostData(url, data, success) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: success,
            });
        },

        ajaxPost(url, success) {
            $.ajax({
                type: "POST",
                url: url,
                success: success
            });
        },

        ajaxGetData(url, data, success) {
            $.ajax({
                type: "GET",
                url: url,
                data: data,
                success: success
            });
        },

        ajaxGet(url, success) {
            $.ajax({
                type: "GET",
                url: url,
                success: success
            });
        },

        getFormData: function (form, parentName = "", parentValue = null, subParentName = "", subParentValue = null) {
            var arr = form.serializeArray(),
                names = (function () {
                    var n = [],
                        l = arr.length - 1;
                    for (; l >= 0; l--) {
                        n.push(arr[l].name);
                    }

                    return n;
                })();

            $('input[type="checkbox"]:not(:checked)').each(function () {
                if ($.inArray(this.name, names) === -1) {
                    arr.push({ name: this.name, value: 'off' });
                }
            });

            if (parentName !== "") {
                var jsonData = {};

                if (!!parentValue) {
                    var func = window[parentValue.split(".")[0]][parentValue.split(".")[1]];
                    var columneValue = func();
                    arr.push({ name: parentName, value: parseInt(columneValue) });
                }

                if (!!subParentValue) {
                    var exists = false;
                    for (var i = 0; i < arr.length; ++i) {
                        if (arr[i].name === subParentName) {
                            arr[i].value = parseInt(subParentValue);
                            exists = true;
                            break;
                        }
                    }

                    if (!exists) {
                        arr.push({ name: subParentName, value: parseInt(subParentValue) });
                    }
                }

            }

            return arr;
        },

        show_grid_validation_errors: function (errors) {
            var validationFormSummary = $('#gridModelEditorBody').find('.validation-summary-valid');
            if (!!validationFormSummary.children()[0]) {
                $(validationFormSummary.children()[0]).empty();
            }
            $.each(errors, function (key, value) {
                var li = $('<li>');
                li.html(value[0].errorMessage);

                $(validationFormSummary.children()[0]).append(li);
            });
        },

        show_validation_errors: function (form, errors) {
            var validationFormSummary = $('#' + form).find('.validation-summary-valid');
            if (!!validationFormSummary.children()[0]) {
                $(validationFormSummary.children()[0]).empty();
            }
            $.each(errors, function (key, value) {
                validationFormSummary.children()[0].append(value[0].errorMessage);
            });

            validationFormSummary.removeClass('hide');
        },

        hide_validation_errors: function (form) {
            var validationFormSummary = $('#' + form).find('.validation-summary-valid');
            if (!!validationFormSummary.children()[0]) {
                $(validationFormSummary.children()[0]).empty();
            }

            validationFormSummary.addClass('hide');
        },

        show_record_saved_notification: function () {
            toastr.success('Record saved successfully.', 'Record saved.', { timeOut: 2000, positionClass: "toast-bottom-right" })
        },

        show_record_deleted_notification: function () {
            toastr.success('Record deleted successfully.', 'Record deleted.', { timeOut: 2000, positionClass: "toast-bottom-right" })
        },

        show_record_error_notification: function (message) {
            toastr.error('Error!.', message, { timeOut: 0, positionClass: "toast-bottom-right", closeButton: true, closeHtml: "<button><i class='fas fa-window-close'></i></button>" })
        },

        init_page: function (e) {

            e.find(".oa_date").each(function () {
                var defaultDate = $(this).attr('defaultDate');
                var format = $(this).attr('format');
                $(this).datetimepicker({
                    defaultDate: defaultDate,
                    format: format
                });
            });

            e.find(".mvc-grid").each(function () {
                new MvcGrid(this);
            });

            var ddls = e.find('.oa_dropdownlist');
            ddls.each(function () {
                oa_dropdownlist.init_ddl(this);
            })
        },

        nameToProperty: function (name) {
            if (name === "") {
                return "";
            } else {
                return name.toLowerCase().split('-').map(w => w.charAt(0).toUpperCase() + w.slice(1)).join('');
            }
        },//new MvcGrid(document.querySelector('#gvPlayers')).columns.map((e) => { return e.name; });



        createPredicate: function (name, filter) {
            var predicate = '';

            if (!!filter && !!filter.first.value) {
                var prop = "m." + oa_utilities.nameToProperty(name);

                var first = oa_utilities.getFilter(filter.first, prop, filter.name);
                var second = '';

                if (!!filter.operator) {
                    var operator = "";

                    switch (filter.operator) {
                        case "and": {
                            operator = "&&";
                            break;
                        }

                        case "or": {
                            operator = "||";
                            break;
                        }
                    }

                    second = " " + operator + " " + oa_utilities.getFilter(filter.second, prop, filter.name);
                }

                predicate = first + second;
            }

            if (!!predicate) {
                predicate = "(" + predicate + ")";
            }

            return predicate;
        },

        getFilter: function (filterOption, name, filterType) {
            if (!!filterOption.value) {
                switch (filterType) {
                    case "number": {
                        switch (filterOption.method) {
                            case "equals": {
                                return name + " == " + filterOption.value;
                            }

                            case "not-equals": {
                                return name + " != " + filterOption.value;
                            }

                            case "less-than": {
                                return name + " < " + filterOption.value;
                            }

                            case "greater-than": {
                                return name + " > " + filterOption.value;
                            }

                            case "less-than-or-equal": {
                                return name + " <= " + filterOption.value;
                            }

                            case "greater-than-or-equal": {
                                return name + " >= " + filterOption.value;
                            }
                        }
                    }

                    case "text": {
                        switch (filterOption.method) {
                            case "contains": {
                                return name + '.ToLower().Contains("' + filterOption.value.toLowerCase() + '")';
                            }

                            case "equals": {
                                return name + '.ToLower() == "' + filterOption.value.toLowerCase() + '"';
                            }

                            case "not-equals": {
                                return "!" + name + '.ToLower().Contains("' + filterOption.value.toLowerCase() + '")';
                            }

                            case "starts-with": {
                                return name + '.ToLower().StartsWith("' + filterOption.value.toLowerCase() + '")';
                            }

                            case "ends-with": {
                                return name + '.ToLower().EndsWith("' + filterOption.value.toLowerCase() + '")';
                            }
                        }
                    }

                    case "date": {
                        //['equals', 'not-equals', 'earlier-than', 'later-than', 'earlier-than-or-equal', 'later-than-or-equal'];
                    }

                    case "boolean": {
                        //['equals', 'not-equals'];
                        switch (filterOption.method) {
                            case "equals": {
                                return name + " == " + filterOption.value.toLowerCase();
                            }

                            case "not-equals": {
                                return name + " != " + filterOption.value.toLowerCase();
                            }
                        }
                    }
                }


            }

        }

    }
})();